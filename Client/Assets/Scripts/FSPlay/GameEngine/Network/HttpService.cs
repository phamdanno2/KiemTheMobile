using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// http方式的资源加载。 该类是异步加载
/// </summary>

public class HttpService : TTMonoBehaviour
{
    static HttpService sInstance = null;
    public const int DEFAULT_TIMEOUT = 30;

    void Awake()
    {
        sInstance = this;
    }

    public static HttpService Instance
    {
        get
        {
            if (sInstance == null)
            {
                GameObject obj = new GameObject();
                obj.name = "HttpServiceObject";
                obj.SetActive(true);
                sInstance = obj.AddComponent<HttpService>();
                GameObject.DontDestroyOnLoad(obj);
            }
            return sInstance;
        }
    }

    //Load callback
    public Action<WWW, object> LoadFinishDelegate = null;
    public Action<WWW> LoadProgressDelegate = null;

    public class URLRequest
    {
        public const int DEFAULT_TIMEOUT = 30;
        public string Url;
        public Action<WWW, object> callbacks;
        public Action<WWW> progressCallbacks;
        public WWW wwwObject = null;
        public WWWForm form = null;
        public List<object> callbackParmas = new List<object>();
        public float startTime = 0f;
        public int Timeout = 0;
  

        public URLRequest(string url, Action<WWW, object> callback, Action<WWW> progressCallback, object param = null, WWWForm form = null,byte[] postData = null, int timeout = 0)
        {
            Url = url;
            callbacks = callback;
            progressCallbacks = progressCallback;
            startTime = Time.realtimeSinceStartup;
            if (timeout <= 0)
            {
                timeout = DEFAULT_TIMEOUT;
            }
            Timeout = timeout;
            if (postData != null)
            {
                //Hashtable hashTable = new Hashtable();
                //hashTable.Add("Content-Type", "application/octet-stream");
                //wwwObject = new WWW(url, postData, hashTable);
                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("Content-Type", "application/octet-stream");
                wwwObject = new WWW(url, postData, dict);
            }
            else if (form != null)
            {
                wwwObject = new WWW(url, form);
            }
            else
            {
                wwwObject = new WWW(url);
            }
            callbackParmas.Add(param);
        }
    }

    private Dictionary<string, URLRequest> DownloadList = new Dictionary<string, URLRequest>();

    void Start()
    {

    }

    public void Load(string url, Action<WWW, object> callback, object param, int timeout = 0)
    {
        if (String.IsNullOrEmpty(url))
        {
            KTDebug.LogError("[Http Service] URL is NULL");
            return;
        }
        if (callback == null)
            callback = (WWW www, object arg) => { };
        if (timeout == null)
            timeout = DEFAULT_TIMEOUT;
        StartCoroutine<bool>(loadFromRemote(url, callback, (WWW arg) => { }, param, null,null, timeout = 0));
    }

    public void Load(string url, Action<WWW, object> callback, byte[] postData, object param, int timeout = 0)
    {
        if (String.IsNullOrEmpty(url))
        {
            KTDebug.LogError("[Http Service] URL is NULL");
            return;
        }
        if (callback == null)
            callback = (WWW www, object arg) => { };
        if (timeout == null)
            timeout = DEFAULT_TIMEOUT;
        StartCoroutine<bool>(loadFromRemote(url, callback, (WWW arg) => { }, param, null, postData, timeout = 0));
    }

    /// <summary>
    /// 发起HTTP请求
    /// </summary>
    /// <param name='url'>
    ///  必选参数
    /// </param>
    /// <param name='callback'>
    ///  完成后的回调,不用时可写null
    /// </param>
    /// <param name='timeout'>
    /// 超时值,单位秒; 默认值 0 (代表30s默认)  超时后callback接收到的 WWW==null  以此判断是否超时
    /// </param>
    public void Load(string url, Action<WWW, object> callback, int timeout)
    {
        if (String.IsNullOrEmpty(url))
        {
            KTDebug.LogError("[Http Service] URL is NULL");
            return;
        }
        if (callback == null)
            callback = (WWW www, object arg) => { };
        if (timeout == null)
            timeout = DEFAULT_TIMEOUT;
        StartCoroutine<bool>(loadFromRemote(url, callback, (WWW arg) => { }, null, null,null, timeout));
    }
    /// <summary>
    /// 发起HTTP请求
    /// </summary>
    /// <param name='url'>
    /// 必选参数
    /// </param>
    /// <param name='callback'>
    /// 完成后的回调,不用时可写null
    /// </param>
    /// <param name='form'>
    /// 表单信息,可为null
    /// </param>
    /// <param name='param'>
    /// 希望完成后传入callback的参数,默认可为null
    /// </param>
    /// <param name='timeout'>
    /// 超时值,单位秒; 默认值 0 (代表30s默认)  超时后callback接收到的 WWW==null  以此判断是否超时
    /// </param>
    public void Load(string url, Action<WWW, object> callback, WWWForm form, object param, int timeout)
    {
        if (String.IsNullOrEmpty(url))
        {
            KTDebug.LogError("[Http Service] URL is NULL");
            return;
        }
        if (callback == null)
            callback = (WWW www, object arg) => { };
        if (timeout == null)
            timeout = DEFAULT_TIMEOUT;
        StartCoroutine<bool>(loadFromRemote(url, callback, (WWW arg) => { }, param, form,null, timeout));
    }
    /// <summary>
    /// 发起HTTP请求
    /// </summary>
    /// <param name='url'>
    /// 必选参数
    /// </param>
    /// <param name='callback'>
    /// 完成后的回调,不用时可写null  回调格式(WWW,object)=>{}
    /// </param>
    /// <param name='progressCallback'>
    /// 进度回调,不用时可写null 回调格式(WWW)=>{} 
    /// </param>
    /// <param name='param'>
    /// 希望完成后传入callback的参数,默认可为null
    /// </param>
    /// <param name='timeout'>
    /// 超时值,单位秒; 默认值 0 (代表30s默认)  超时后callback接收到的 WWW==null  以此判断是否超时
    /// </param>
    public void Load(string url, Action<WWW, object> callback, Action<WWW> progressCallback, object param, int timeout = 0)
    {
        if (String.IsNullOrEmpty(url))
        {
            KTDebug.LogError("[Http Service] URL is NULL");
            return;
        }
        if (callback == null)
            callback = (WWW www, object arg) => { };
        if (progressCallback == null)
            progressCallback = (WWW arg) => { };
        if (timeout == 0)
            timeout = DEFAULT_TIMEOUT;
        StartCoroutine<bool>(loadFromRemote(url, callback, progressCallback, param, null,null, timeout));
    }

    IEnumerator loadFromRemote(string url, Action<WWW, object> callback, Action<WWW> progressCallback, object param = null, WWWForm form = null, byte[] postData = null,int timeout = 0)
    {
        yield return 1;
        if (DownloadList.ContainsKey(url))
        {
            DownloadList[url].callbacks += callback;
            DownloadList[url].progressCallbacks += progressCallback;
            DownloadList[url].callbackParmas.Add(param);
        }
        else
        {
            DownloadList.Add(url, new URLRequest(url, callback, progressCallback, param, form, postData, timeout));
        }
    }
    int oldTime = 0;
    void Update()
    {
        /*if(((int)Time.realtimeSinceStartup - oldTime)< 1 )
        {
            return;
        }
        oldTime  = (int)Time.realtimeSinceStartup;
		
        KTDebug.Log("Time"+Time.realtimeSinceStartup);*/
        List<string> completedItems = null;

        foreach (KeyValuePair<string, URLRequest> wwwPair in DownloadList)
        {
            URLRequest wwwReq = wwwPair.Value;

            if (wwwReq.wwwObject.isDone)
            {
                //下载完成
                Action<WWW, object> callbacks = (Action<WWW, object>)wwwReq.callbacks;
                while (callbacks != null && callbacks.GetInvocationList().GetLength(0) > 0)
                {
                    Action<WWW, object> callback = (Action<WWW, object>)callbacks.GetInvocationList()[0];
                    if (!string.IsNullOrEmpty(wwwReq.wwwObject.error ))
                    {
                        KTDebug.LogWarning(this.name + " : " + wwwReq.wwwObject.error + " , " + wwwReq.Url);
                    }
                    try
                    {
                        callbacks -= callback;
                        //销毁进度监听
                        //(Action<WWW>)wwwReq.progressCallbacks -= (Action<WWW>)wwwReq.progressCallbacks.GetInvocationList()[0];
                        wwwReq.progressCallbacks -= (Action<WWW>)wwwReq.progressCallbacks.GetInvocationList()[0];
                        callback.Invoke(wwwReq.wwwObject, wwwReq.callbackParmas[0]);
                    }
                    catch (Exception e)
                    {
                        KTDebug.LogException(e);
                    }

                    if (wwwReq.callbackParmas.Count > 0)
                    {
                        wwwReq.callbackParmas.RemoveAt(0);
                    }
                }

                if (completedItems == null)
                {
                    completedItems = new List<string>();
                }
                completedItems.Add(wwwPair.Key);
            }
            else
            {
                //下载过程中  
                if (wwwReq.progressCallbacks != null)
                {
                    try
                    {
                        if (Time.realtimeSinceStartup - wwwReq.startTime > wwwReq.Timeout)
                        {
                            //超时处理
                            KTDebug.LogWarning("[Http Service]" + wwwReq.Url + ", TIMEOUT");

                            //下载完成
                            Action<WWW, object> callbacks = (Action<WWW, object>)wwwReq.callbacks;
                            while (callbacks != null && callbacks.GetInvocationList().GetLength(0) > 0)
                            {
                                Action<WWW, object> callback = (Action<WWW, object>)callbacks.GetInvocationList()[0];
                                if (!string.IsNullOrEmpty(wwwReq.wwwObject.error))
                                {
                                    KTDebug.LogWarning(wwwReq.wwwObject.error);
                                }
                                try
                                {
                                    callbacks -= callback;
                                    //销毁进度监听
                                    //(Action<WWW>)wwwReq.progressCallbacks -= (Action<WWW>)wwwReq.progressCallbacks.GetInvocationList()[0];
                                    wwwReq.progressCallbacks -= (Action<WWW>)wwwReq.progressCallbacks.GetInvocationList()[0];
                                    callback.Invoke(null, wwwReq.Url);
                                }
                                catch (Exception e)
                                {
                                    KTDebug.LogException(e);
                                }

                                if (wwwReq.callbackParmas.Count > 0)
                                {
                                    wwwReq.callbackParmas.RemoveAt(0);
                                }
                            }

                            if (completedItems == null)
                            {
                                completedItems = new List<string>();
                            }
                            completedItems.Add(wwwPair.Key);
                        }
                        else
                        {
                            //更新进度
                            //KTDebug.LogWarning("^^^"+wwwReq.Url+","+wwwReq.Timeout+","+(Time.realtimeSinceStartup-wwwReq.startTime).ToString());
                            ((Action<WWW>)wwwReq.progressCallbacks.GetInvocationList()[0]).Invoke(wwwReq.wwwObject);
                        }

                    }
                    catch (Exception e)
                    {
                        KTDebug.LogException(e);
                    }
                }
            }

        }

        if (completedItems != null)
        {
            for (int i = 0; i < completedItems.Count; i++)
            {
                DownloadList.Remove(completedItems[i]);
            }
            completedItems.Clear();
            completedItems = null;
        }

    }

}