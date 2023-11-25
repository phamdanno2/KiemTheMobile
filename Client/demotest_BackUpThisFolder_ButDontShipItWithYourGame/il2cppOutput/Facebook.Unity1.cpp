#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <limits>


template <typename T1>
struct VirtualActionInvoker1
{
	typedef void (*Action)(void*, T1, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		((Action)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
template <typename R, typename T1>
struct VirtualFuncInvoker1
{
	typedef R (*Func)(void*, T1, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_virtual_invoke_data(slot, obj);
		return ((Func)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
struct InterfaceActionInvoker0
{
	typedef void (*Action)(void*, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, invokeData.method);
	}
};
template <typename T1>
struct InterfaceActionInvoker1
{
	typedef void (*Action)(void*, T1, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
template <typename T1, typename T2>
struct InterfaceActionInvoker2
{
	typedef void (*Action)(void*, T1, T2, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1, T2 p2)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, p1, p2, invokeData.method);
	}
};
template <typename T1, typename T2, typename T3>
struct InterfaceActionInvoker3
{
	typedef void (*Action)(void*, T1, T2, T3, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1, T2 p2, T3 p3)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, p1, p2, p3, invokeData.method);
	}
};
template <typename T1, typename T2, typename T3, typename T4, typename T5>
struct InterfaceActionInvoker5
{
	typedef void (*Action)(void*, T1, T2, T3, T4, T5, const RuntimeMethod*);

	static inline void Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		((Action)invokeData.methodPtr)(obj, p1, p2, p3, p4, p5, invokeData.method);
	}
};
template <typename R>
struct InterfaceFuncInvoker0
{
	typedef R (*Func)(void*, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		return ((Func)invokeData.methodPtr)(obj, invokeData.method);
	}
};
template <typename R, typename T1>
struct InterfaceFuncInvoker1
{
	typedef R (*Func)(void*, T1, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		return ((Func)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};
template <typename R, typename T1, typename T2>
struct InterfaceFuncInvoker2
{
	typedef R (*Func)(void*, T1, T2, const RuntimeMethod*);

	static inline R Invoke (Il2CppMethodSlot slot, RuntimeClass* declaringInterface, RuntimeObject* obj, T1 p1, T2 p2)
	{
		const VirtualInvokeData& invokeData = il2cpp_codegen_get_interface_invoke_data(slot, obj, declaringInterface);
		return ((Func)invokeData.methodPtr)(obj, p1, p2, invokeData.method);
	}
};
template <typename R, typename T1>
struct GenericInterfaceFuncInvoker1
{
	typedef R (*Func)(void*, T1, const RuntimeMethod*);

	static inline R Invoke (const RuntimeMethod* method, RuntimeObject* obj, T1 p1)
	{
		VirtualInvokeData invokeData;
		il2cpp_codegen_get_generic_interface_invoke_data(method, obj, &invokeData);
		return ((Func)invokeData.methodPtr)(obj, p1, invokeData.method);
	}
};

// Facebook.Unity.Utilities/Callback`1<System.Object>
struct Callback_1_t244FCFB1C42CF22CE5AF6954370376FF384F449A;
// Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer>
struct Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92;
// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IAppRequestResult>
struct CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE;
// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IPayResult>
struct CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6;
// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IShareResult>
struct CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052;
// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<System.Object>
struct CanvasUIMethodCall_1_t4DF5B3B0E021839100A7F808C6A6A596BC1405C3;
// System.Collections.Generic.Dictionary`2<System.Int32,System.Globalization.CultureInfo>
struct Dictionary_2_t9FA6D82CAFC18769F7515BB51D1C56DAE09381C3;
// System.Collections.Generic.Dictionary`2<System.Object,System.Object>
struct Dictionary_2_t14FE4A752A83D53771C584E4C8D14E01F2AFD7BA;
// System.Collections.Generic.Dictionary`2<System.String,System.Globalization.CultureInfo>
struct Dictionary_2_tE1603CE612C16451D1E56FF4D4859D4FE4087C28;
// System.Collections.Generic.Dictionary`2<System.String,System.Object>
struct Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710;
// System.Collections.Generic.Dictionary`2<System.String,System.String>
struct Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAccessTokenRefreshResult>
struct FacebookDelegate_1_t0A787A64D3187E98A865A198DDF444C008C79F35;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppLinkResult>
struct FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppRequestResult>
struct FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ICatalogResult>
struct FacebookDelegate_1_tA8F51BBA2E7364881368E0CBED892F561162C32E;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IConsumePurchaseResult>
struct FacebookDelegate_1_tF96C838D4977CEFDB874E77F7A56B1CFECF4D8DD;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGamingServicesFriendFinderResult>
struct FacebookDelegate_1_t55619BCDC758CB1C82277E6D8BA44ACA47C03DE2;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGraphResult>
struct FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IIAPReadyResult>
struct FacebookDelegate_1_tA2A619ACCF137D8094955A556E78A66749C43F74;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInitCloudGameResult>
struct FacebookDelegate_1_tB829CDA8266CAB15D1703F5904BB2F8592CA60E8;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInterstitialAdResult>
struct FacebookDelegate_1_t6BAE034F2CC3270BFC282A9D0DEB58CC5F91C265;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ILoginResult>
struct FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IMediaUploadResult>
struct FacebookDelegate_1_t7CA00F6A27B3FE85139590EFEA03B7C7C0D4A66D;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IOpenAppStoreResult>
struct FacebookDelegate_1_t5E457BF6B03D4AD72D97F1848A0612B675747CFD;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayResult>
struct FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayloadResult>
struct FacebookDelegate_1_tB6B04FA63685B9A0D41AB08E3E5BC96A72D7A1D7;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchaseResult>
struct FacebookDelegate_1_tBD274D4AED39A6A24A22981D38D3CBF447CF036E;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchasesResult>
struct FacebookDelegate_1_t61D056F5D405CB3DF2F643449D9C03A64AB8E818;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IResult>
struct FacebookDelegate_1_tD24CEC09066861D3AC22EC3D0CFC1542F4592B1D;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IRewardedVideoResult>
struct FacebookDelegate_1_t9C5748E8AC36242F3F03171AA1E6306E60860ACD;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IScheduleAppToUserNotificationResult>
struct FacebookDelegate_1_t623E520D5B99523D410AEE72AB9FF4901DC356AC;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ISessionScoreResult>
struct FacebookDelegate_1_t610D359C8E47669CC0D9F0B6CA9DF9240BF80267;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IShareResult>
struct FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentResult>
struct FacebookDelegate_1_tD9C9BBFE6AD0931C935E391D858C37BEBF14BBA3;
// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentScoreResult>
struct FacebookDelegate_1_t474682078C474498C8D4805F13E9077763B39255;
// Facebook.Unity.FacebookDelegate`1<System.Object>
struct FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9;
// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>,System.Object>
struct Func_2_tF42287527472FA89789873F068A87C60A00EC7D3;
// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.Object>
struct Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF;
// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.String>
struct Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088;
// System.Collections.Generic.IDictionary`2<System.String,System.Object>
struct IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19;
// System.Collections.Generic.IDictionary`2<System.String,System.String>
struct IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF;
// System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>>
struct IEnumerable_1_t60509816D8966320E2A9660FC756B6C440ADFC50;
// System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.String>>
struct IEnumerable_1_t3C6913E067AB1171D9894C79A396D8A8E90E311B;
// System.Collections.Generic.IEnumerable`1<System.Object>
struct IEnumerable_1_tF95C9E01A913DD50575531C8305932628663D9E9;
// System.Collections.Generic.IEnumerable`1<System.String>
struct IEnumerable_1_t349E66EC5F09B881A8E52EE40A1AB9EC60E08E44;
// System.Collections.Generic.IEqualityComparer`1<System.String>
struct IEqualityComparer_1_tAE94C8F24AD5B94D4EE85CA9FC59E3409D41CAF7;
// System.Collections.Generic.IList`1<System.Object>
struct IList_1_t6EE90D273EFCF5E7E4C37FAB712E70BB6F1B4BFF;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>
struct JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppLinkResult>
struct JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppRequestResult>
struct JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ICatalogResult>
struct JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IConsumePurchaseResult>
struct JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>
struct JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IIAPReadyResult>
struct JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInitCloudGameResult>
struct JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInterstitialAdResult>
struct JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ILoginResult>
struct JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IMediaUploadResult>
struct JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IOpenAppStoreResult>
struct JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPayloadResult>
struct JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchaseResult>
struct JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchasesResult>
struct JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IResult>
struct JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IRewardedVideoResult>
struct JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>
struct JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ISessionScoreResult>
struct JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IShareResult>
struct JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentResult>
struct JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentScoreResult>
struct JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557;
// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<System.Object>
struct JavaMethodCall_1_t27CF0C3D13B160B267BA993DED70243E1963EE60;
// System.Collections.Generic.Dictionary`2/KeyCollection<System.String,System.Object>
struct KeyCollection_tE66790F09E854C19C7F612BEAD203AE626E90A36;
// System.Collections.Generic.Dictionary`2/KeyCollection<System.String,System.String>
struct KeyCollection_t2EDD317F5771E575ACB63527B5AFB71291040342;
// System.Collections.Generic.List`1<System.Object>
struct List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D;
// System.Collections.Generic.List`1<System.String>
struct List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>
struct MethodCall_1_t1852FDCA7708C74BD9575219E6FC7F609E56BA8E;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IAppLinkResult>
struct MethodCall_1_t73261478565B64ABA19B127D15BD7DCCD81D6A06;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IAppRequestResult>
struct MethodCall_1_t1D2B62ADEB209FD37D3B0AED0788B5A36C325DED;
// Facebook.Unity.MethodCall`1<Facebook.Unity.ICatalogResult>
struct MethodCall_1_tA51B63B488D06F9DD4269E5D936844297AC2F717;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IConsumePurchaseResult>
struct MethodCall_1_t170D11D6663ABDBA2714482029BC9107E6119F32;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>
struct MethodCall_1_t28181D9D5D0204C879A034D9757AF1F63C3EC7AF;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IIAPReadyResult>
struct MethodCall_1_t3129C45AEAB38F50384EF247D7ED9921171D1DF7;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IInitCloudGameResult>
struct MethodCall_1_tFDD6DDBFBE6A9432424C4841D30F5018A40CFEDC;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IInterstitialAdResult>
struct MethodCall_1_t7E8B67FDC468ABDD86D3E481B50034289CB7279D;
// Facebook.Unity.MethodCall`1<Facebook.Unity.ILoginResult>
struct MethodCall_1_tFC828FE9EEAFBD29ACB6560A77095DF9CC02959F;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IMediaUploadResult>
struct MethodCall_1_tACE924324819B8E5DAC6615D0A058E16C5A7C729;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IOpenAppStoreResult>
struct MethodCall_1_t87E54F439C1534715A53A604C0EAE5850B55120D;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IPayResult>
struct MethodCall_1_t222223E3C8CAEF78C55722F5474C0C2C66F0EA2B;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IPayloadResult>
struct MethodCall_1_tDD7FB7C96BB6551BC13B0499833326C4B55CB468;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchaseResult>
struct MethodCall_1_tE2E7E098CBA48E270C26249E671C3D59EB7D0C27;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchasesResult>
struct MethodCall_1_t964EEAC4E9A2090141E286A38DD20AD5D31C42B3;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IRewardedVideoResult>
struct MethodCall_1_tDFA97C21B6B2B2BC255F28E227D0F5FA5829F686;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>
struct MethodCall_1_t619F8AD0F81AEF389F5C6063A4C1F8C06036B029;
// Facebook.Unity.MethodCall`1<Facebook.Unity.ISessionScoreResult>
struct MethodCall_1_t3A25163740B62335265043F26060847249D6C047;
// Facebook.Unity.MethodCall`1<Facebook.Unity.IShareResult>
struct MethodCall_1_t0D27E61246A32F94E6F80F665C505FC54279576C;
// Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentResult>
struct MethodCall_1_tAE6A4425ED88803929C6E6458A9EF1A83135B315;
// Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentScoreResult>
struct MethodCall_1_tACF312248F1E681E1DA6AF486A149C37756A81E3;
// Facebook.Unity.MethodCall`1<System.Object>
struct MethodCall_1_tBE309A8BE882A6A1F80E42A8EE1493B9CB4FBBC8;
// UnityEngine.Events.UnityAction`2<UnityEngine.SceneManagement.Scene,System.Int32Enum>
struct UnityAction_2_tF47D82C7E3C3B118B409866D926435B55A0675BD;
// UnityEngine.Events.UnityAction`2<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>
struct UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A;
// System.Collections.Generic.Dictionary`2/ValueCollection<System.String,System.Object>
struct ValueCollection_tC9D91E8A3198E40EA339059703AB10DFC9F5CC2E;
// System.Collections.Generic.Dictionary`2/ValueCollection<System.String,System.String>
struct ValueCollection_t238D0D2427C6B841A01F522A41540165A2C4AE76;
// System.Collections.Generic.Dictionary`2/Entry<System.String,System.Object>[]
struct EntryU5BU5D_t233BB24ED01E2D8D65B0651D54B8E3AD125CAF96;
// System.Collections.Generic.Dictionary`2/Entry<System.String,System.String>[]
struct EntryU5BU5D_t1AF33AD0B7330843448956EC4277517081658AE7;
// System.Byte[]
struct ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031;
// System.Char[]
struct CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB;
// System.Delegate[]
struct DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771;
// System.Int32[]
struct Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C;
// System.IntPtr[]
struct IntPtrU5BU5D_tFD177F8C806A6921AD7150264CCC62FA00CAD832;
// System.Object[]
struct ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918;
// System.Diagnostics.StackTrace[]
struct StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF;
// System.String[]
struct StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248;
// System.Type[]
struct TypeU5BU5D_t97234E1129B564EB38B8D85CAC2AD8B5B9522FFB;
// Facebook.Unity.AccessToken
struct AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346;
// Facebook.Unity.Mobile.Android.AndroidFacebook
struct AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD;
// Facebook.Unity.Mobile.Android.AndroidFacebookGameObject
struct AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378;
// Facebook.Unity.Mobile.Android.AndroidFacebookLoader
struct AndroidFacebookLoader_t581FB6217BD8B55FE1012FCF1AC8EDABE25320E9;
// Facebook.Unity.AppLinkResult
struct AppLinkResult_t7B3A5BB1C71DF6D48BBC7B508AE365E8D4A79143;
// Facebook.Unity.AppRequestResult
struct AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2;
// System.Reflection.Assembly
struct Assembly_t;
// Facebook.Unity.AuthenticationToken
struct AuthenticationToken_t925F4C42BE7D08897D579F0A55D5682704237B8C;
// System.Reflection.Binder
struct Binder_t91BFCE95A7057FADF4D8A1A342AFE52872246235;
// System.Globalization.Calendar
struct Calendar_t0A117CC7532A54C17188C2EFEA1F79DB20DF3A3B;
// Facebook.Unity.CallbackManager
struct CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749;
// Facebook.Unity.Canvas.CanvasFacebook
struct CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1;
// Facebook.Unity.Canvas.CanvasFacebookGameObject
struct CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46;
// Facebook.Unity.Canvas.CanvasFacebookLoader
struct CanvasFacebookLoader_t50D8266C527416BEBCBA1355E4513AD3DEBC71EA;
// System.Globalization.CompareInfo
struct CompareInfo_t1B1A6AC3486B570C76ABA52149C9BD4CD82F9E57;
// UnityEngine.Component
struct Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3;
// System.Globalization.CultureData
struct CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D;
// System.Globalization.CultureInfo
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0;
// System.Globalization.DateTimeFormatInfo
struct DateTimeFormatInfo_t0457520F9FA7B5C8EAAEB3AD50413B6AEEB7458A;
// System.DelegateData
struct DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E;
// System.Enum
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2;
// Facebook.Unity.FBLocation
struct FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236;
// Facebook.Unity.FacebookBase
struct FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865;
// Facebook.Unity.FacebookGameObject
struct FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B;
// UnityEngine.GameObject
struct GameObject_t76FEDD663AB33C991A9C9A23129337651094216F;
// Facebook.Unity.HideUnityDelegate
struct HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE;
// Facebook.Unity.IAccessTokenRefreshResult
struct IAccessTokenRefreshResult_tCE03B5E5D705D890DA53377D57A576F65D25EA0F;
// Facebook.Unity.Mobile.Android.IAndroidWrapper
struct IAndroidWrapper_t3D8FC6369EBCAB7A86AE6A71C08CD96C9E1DDC63;
// Facebook.Unity.IAppLinkResult
struct IAppLinkResult_t179F2EFCB4C2DB9508A03865834BEA504D186CFF;
// Facebook.Unity.IAppRequestResult
struct IAppRequestResult_t7BE3B2F98507199EAFA29911A7053FBD2F1033FE;
// Facebook.Unity.Canvas.ICanvasFacebookCallbackHandler
struct ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271;
// Facebook.Unity.Canvas.ICanvasFacebookImplementation
struct ICanvasFacebookImplementation_t26FCCCC1DEF6E943A189D145F7FB7E4E5B90A38C;
// Facebook.Unity.Canvas.ICanvasJSWrapper
struct ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12;
// Facebook.Unity.ICatalogResult
struct ICatalogResult_t7B2754FE09EA3E0E3C1A9200DE6781900C6535EF;
// Facebook.Unity.IConsumePurchaseResult
struct IConsumePurchaseResult_tD63F386CC1C86E8C04AD8D3FB661BDB935E7366E;
// System.Collections.IDictionary
struct IDictionary_t6D03155AF1FA9083817AA5B6AD7DEEACC26AB220;
// Facebook.Unity.IFacebook
struct IFacebook_t9EF67C093B6D1642C45FB2344C8E7EE840AAF324;
// Facebook.Unity.IFacebookImplementation
struct IFacebookImplementation_t812E9B44E4C2420A1D7A3C4E6834C5D9C3B44748;
// System.IFormatProvider
struct IFormatProvider_tC202922D43BFF3525109ABF3FB79625F5646AB52;
// Facebook.Unity.IGamingServicesFriendFinderResult
struct IGamingServicesFriendFinderResult_tD05219A80D17817C36922D35F12038705DD72EFD;
// Facebook.Unity.IGraphResult
struct IGraphResult_t1DDA97077163449C229F90EC960384F016AF2BB6;
// Facebook.Unity.IIAPReadyResult
struct IIAPReadyResult_tBDC1CF4CEB4E942820A8AD4024A493821EC1DCCE;
// Facebook.Unity.Mobile.IOS.IIOSWrapper
struct IIOSWrapper_t40FDB67E3759959235A21CA13F14028147479E3F;
// Facebook.Unity.IInitCloudGameResult
struct IInitCloudGameResult_t69068E1F2A1601B3F4F312FBDDD309933519C23C;
// Facebook.Unity.IInternalResult
struct IInternalResult_tF41EEB8974D2FA61D5379AD86B93FEE2D7120560;
// Facebook.Unity.IInterstitialAdResult
struct IInterstitialAdResult_tBB92461B2F4D5225AFF7D88F57664ABE6537DA7C;
// Facebook.Unity.ILoginResult
struct ILoginResult_t809F9817555CF3FF1F2C11154D110DB3F55C07ED;
// Facebook.Unity.IMediaUploadResult
struct IMediaUploadResult_tD2E622068BC3194399FE8317B031B5B886F83DEF;
// Facebook.Unity.Mobile.IOS.IOSFacebook
struct IOSFacebook_tC5DD1DAB3274516F7194A166C4C9D5C2900A99F4;
// Facebook.Unity.Mobile.IOS.IOSFacebookGameObject
struct IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA;
// Facebook.Unity.Mobile.IOS.IOSFacebookLoader
struct IOSFacebookLoader_tC4449925F5E12605B6DAC60A202C0EC3ECB7B242;
// Facebook.Unity.IOpenAppStoreResult
struct IOpenAppStoreResult_tE5B82F948D89EED0507904C6F27B72DF13DF08D3;
// Facebook.Unity.IPayResult
struct IPayResult_t4E63FC9B25853087E753551D5097B1CC1574894B;
// Facebook.Unity.IPayloadResult
struct IPayloadResult_t5B8BD11353F67B64FE2F49CD99F96F5DC38483BB;
// Facebook.Unity.IPurchaseResult
struct IPurchaseResult_tA639FCD3903E9389DFFE5DA702475E54333FE0DA;
// Facebook.Unity.IPurchasesResult
struct IPurchasesResult_t414729B48C8387EFB70B9FB1BB0A02F9E8571469;
// Facebook.Unity.IRewardedVideoResult
struct IRewardedVideoResult_t1C1F9B00A4902AD09334288BDCA507FF5AEFB26D;
// Facebook.Unity.IScheduleAppToUserNotificationResult
struct IScheduleAppToUserNotificationResult_tBC09DCB04A5A2A48F7337D66573E443F408C8927;
// Facebook.Unity.ISessionScoreResult
struct ISessionScoreResult_t59D9339531BF2F355D70388845F271FC386B3C72;
// Facebook.Unity.IShareResult
struct IShareResult_tE0B3DB4E03C8F0EC5082F757426F6C24E7D94DD8;
// Facebook.Unity.ITournamentResult
struct ITournamentResult_tA1276E918CE4CB282B94BC785B3B5EB41CBB2284;
// Facebook.Unity.ITournamentScoreResult
struct ITournamentScoreResult_tB5E0986FA3B48F7181C58C1D2E8B8D45E02ED554;
// Facebook.Unity.InitDelegate
struct InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F;
// Facebook.Unity.Canvas.JsBridge
struct JsBridge_t338341DF8272C935803F827012749337260971DA;
// Facebook.Unity.LoginResult
struct LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1;
// Facebook.Unity.LoginStatusResult
struct LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7;
// System.Reflection.MemberFilter
struct MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553;
// Facebook.Unity.MethodArguments
struct MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD;
// System.Reflection.MethodInfo
struct MethodInfo_t;
// Facebook.Unity.Mobile.MobileFacebook
struct MobileFacebook_tFB7FCB79FF54D81C7EB30A5D40786FAB1DBFB21E;
// Facebook.Unity.Mobile.MobileFacebookGameObject
struct MobileFacebookGameObject_t105BCEF7BF0751B141C332580EE375559D10BB7C;
// UnityEngine.MonoBehaviour
struct MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71;
// System.NotImplementedException
struct NotImplementedException_t6366FE4DCF15094C51F4833B91A2AE68D4DA90E8;
// System.Globalization.NumberFormatInfo
struct NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472;
// Facebook.Unity.PayResult
struct PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08;
// Facebook.Unity.Profile
struct Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7;
// Facebook.Unity.ResultContainer
struct ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B;
// System.Runtime.Serialization.SafeSerializationManager
struct SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6;
// Facebook.Unity.ShareResult
struct ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0;
// System.String
struct String_t;
// System.Globalization.TextInfo
struct TextInfo_tD3BAFCFD77418851E7D5CB8D2588F47019E414B4;
// UnityEngine.Transform
struct Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1;
// System.Type
struct Type_t;
// System.Uri
struct Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E;
// System.UriParser
struct UriParser_t920B0868286118827C08B08A15A9456AF6C19D81;
// Facebook.Unity.UserAgeRange
struct UserAgeRange_t3074872BAC5CB213D6E7245C5C9407CA12B7321A;
// System.Void
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;
// Facebook.Unity.Mobile.Android.AndroidFacebook/<>c
struct U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD;
// Facebook.Unity.Canvas.CanvasFacebook/<>c
struct U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8;
// Facebook.Unity.Canvas.CanvasFacebook/<>c__DisplayClass47_0
struct U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A;
// Facebook.Unity.FB/CompiledFacebookLoader
struct CompiledFacebookLoader_tDDF38F9F03C32FC27FB7546387BA7B9DBDCF5089;
// Facebook.Unity.FB/OnDLLLoaded
struct OnDLLLoaded_t9F6A891D0400EBD1F1267ED3C20ABDCEBCA8DED7;
// Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict
struct NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9;
// System.Uri/UriInfo
struct UriInfo_t5F91F77A93545DDDA6BB24A609BAF5E232CC1A09;

IL2CPP_EXTERN_C RuntimeClass* AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* AppLinkResult_t7B3A5BB1C71DF6D48BBC7B508AE365E8D4A79143_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Debug_t8394C7EEAECA3689C2C9B9DE9C7166D73596276F_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Exception_t_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* FB_tD6AF917A642BEC6920761C8E4AD4013414829013_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* FacebookLogger_tAF941780684648D82AFA6B5D95965FAAFBBDB457_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* FacebookUnityPlatform_tC5501D323BFD32B9D365B86D368A28653262A4D9_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* GameObject_t76FEDD663AB33C991A9C9A23129337651094216F_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IAndroidWrapper_t3D8FC6369EBCAB7A86AE6A71C08CD96C9E1DDC63_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ICanvasFacebookImplementation_t26FCCCC1DEF6E943A189D145F7FB7E4E5B90A38C_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ICanvasFacebookResultHandler_t0A9297E8C6A10D56FF83AAF579422BC153AAE54E_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ICollection_1_t5C03FBFD5ECBDE4EAB8C4ED582DDFCF702EB5DC7_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IDisposable_t030E0496B4E0E4E4F086825007979AF51F7248C5_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IEnumerable_1_t1F32F711C91AEBCFA4770668CA067447D2A4F665_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IEnumerable_1_tF95C9E01A913DD50575531C8305932628663D9E9_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IEnumerator_1_t43D2E4BA9246755F293DFA74F001FB1A70A648FD_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IEnumerator_1_t913F242211877D391217C9D75152235266FDAF10_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IEnumerator_t7B609C2FFA6EB5167D9C62A0C32A21DE2F666DAA_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IFacebookCallbackHandler_t3D24BE45B97C4B3E6BD94E249E3E9D0C27CC914D_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IOSFacebook_tC5DD1DAB3274516F7194A166C4C9D5C2900A99F4_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* IResult_tDB97C9EEF7759E32ACC78D2DD9B7B0D4E42B67B6_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Json_t691FA1D65828CDA57416D327F6CBA3C911ABCE1E_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* NotImplementedException_t6366FE4DCF15094C51F4833B91A2AE68D4DA90E8_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* SceneManager_tA0EF56A88ACA4A15731AF7FDC10A869FA4C698FA_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ShareDialogMode_t06BC7B4180D56064F766F1F7D505594B59279523_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* String_t_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C String_t* _stringLiteral01C8B71057B916ADBDD8F28D026373D762E66FEF;
IL2CPP_EXTERN_C String_t* _stringLiteral02E19B255FDA406167DD7B953DDD571E02DEE7DD;
IL2CPP_EXTERN_C String_t* _stringLiteral03AF4AAE45F0FD9CE9D36A119A4A931D2A7620AD;
IL2CPP_EXTERN_C String_t* _stringLiteral0458562336F91AC3F0C3FE71A886E75CE5C8F84A;
IL2CPP_EXTERN_C String_t* _stringLiteral0B09BDDCF49998D084C7F5812B74CC75E585E07E;
IL2CPP_EXTERN_C String_t* _stringLiteral126A04A5A066C00B77AB4F3483884A7606F6BC5D;
IL2CPP_EXTERN_C String_t* _stringLiteral1317CF02F3F3926703DF869C594244C35D400F6A;
IL2CPP_EXTERN_C String_t* _stringLiteral1405C2A661574468F6107DE8ADDF274A347D4F54;
IL2CPP_EXTERN_C String_t* _stringLiteral1603A385DE0A7E3A96A39790B8CB10F554D4C836;
IL2CPP_EXTERN_C String_t* _stringLiteral1630E6A6E4B065CB228F2BB0735FC4EB04ADCF98;
IL2CPP_EXTERN_C String_t* _stringLiteral1696D3821CE92FE7EA9D4A8DA8A1CABCC6A3DA40;
IL2CPP_EXTERN_C String_t* _stringLiteral18F572556D6BF6632E36F7AEF2204E0BB1DCD4D3;
IL2CPP_EXTERN_C String_t* _stringLiteral1A0B9E6C938C3916F9D9E2D5124BD4B98AFF0556;
IL2CPP_EXTERN_C String_t* _stringLiteral1C9D772538D2CCB196770E5E5AB06FBE4FBFD01F;
IL2CPP_EXTERN_C String_t* _stringLiteral1CF8013125B2A6F1169557DB315E42C0E45A87C5;
IL2CPP_EXTERN_C String_t* _stringLiteral1D692E10D206F441AFD73AB51AFABF2D76739E52;
IL2CPP_EXTERN_C String_t* _stringLiteral1DDC3BD3066DA80913E08F31F0BE455DC5ECF9A3;
IL2CPP_EXTERN_C String_t* _stringLiteral1DF53E67AAE9B13FCBFF7748CBF5FD7DEA2088B6;
IL2CPP_EXTERN_C String_t* _stringLiteral1F751E5A1E5A1E5CE85E82DD29AF64763DD5DA62;
IL2CPP_EXTERN_C String_t* _stringLiteral2400E7BE5B6DFE8885596899EABA1AE106FE5ED8;
IL2CPP_EXTERN_C String_t* _stringLiteral24187FD658C1381615A4926BBAEE466B99119C27;
IL2CPP_EXTERN_C String_t* _stringLiteral28CD321C015FD8117CD1B202457F438D5669BE3C;
IL2CPP_EXTERN_C String_t* _stringLiteral2B8E19EBECECBC918E301E45BEEE7CE08295EF89;
IL2CPP_EXTERN_C String_t* _stringLiteral2BEF6C0728DEA9AE2CE7B588B768DE3FC7583CC9;
IL2CPP_EXTERN_C String_t* _stringLiteral30A7BF1FEE5DF8662ED83E80AA4537E34CC0361B;
IL2CPP_EXTERN_C String_t* _stringLiteral30ADD6F92E008EF7749F86EFEC1366D210E74DF1;
IL2CPP_EXTERN_C String_t* _stringLiteral30F20681B7D1BDA035E4777AE6F8EF22F97224FC;
IL2CPP_EXTERN_C String_t* _stringLiteral31D307275CC464AFDCC4A193A3D0DADE7D308F81;
IL2CPP_EXTERN_C String_t* _stringLiteral336EA4DDA4EC10AC44C546769AC33BC45133C712;
IL2CPP_EXTERN_C String_t* _stringLiteral33846201AA643F30E3A9FE0E17D65FD9F27C98A7;
IL2CPP_EXTERN_C String_t* _stringLiteral363C72B549019095B81245E1ADCD3F7DE027672D;
IL2CPP_EXTERN_C String_t* _stringLiteral364F4173856E05DF96506EB76D1DECAD55D36048;
IL2CPP_EXTERN_C String_t* _stringLiteral3C74EE53B1AF65557F9BDF1EAF0C416BADC79DB9;
IL2CPP_EXTERN_C String_t* _stringLiteral3CCA51B7FCAD5C35FAA203055F3EE4C112A51BDA;
IL2CPP_EXTERN_C String_t* _stringLiteral3DA85F3B75DC932851E7BA8C186F00E264675CBB;
IL2CPP_EXTERN_C String_t* _stringLiteral3F3FFF195FC51E628335B6613953826BB06DAEB4;
IL2CPP_EXTERN_C String_t* _stringLiteral415902D5F52543D5ED4C8B796FB49950BFA51EF0;
IL2CPP_EXTERN_C String_t* _stringLiteral421262BE3B7342276C4DBB494A8258DF4A2AE9FD;
IL2CPP_EXTERN_C String_t* _stringLiteral44B679A4FEA54FC0DBB1CECD512FC3FCFFE445F4;
IL2CPP_EXTERN_C String_t* _stringLiteral4502F3B52C9C8E53863433138E98E29E02FA04DB;
IL2CPP_EXTERN_C String_t* _stringLiteral46C0DBF8E329C836732C7C6705567D71C6792B50;
IL2CPP_EXTERN_C String_t* _stringLiteral49DF6F7C16A7531B4C585988196CC8F4BAECBE73;
IL2CPP_EXTERN_C String_t* _stringLiteral49FDF777B468FCDB3266D04859340340232DD589;
IL2CPP_EXTERN_C String_t* _stringLiteral4BB4572C6AC73940D49948BC96E824C0CFA913C8;
IL2CPP_EXTERN_C String_t* _stringLiteral4BDCC8C1F6304193EA13F4AFB26677B5B8AF161A;
IL2CPP_EXTERN_C String_t* _stringLiteral4CEA8A0063213A8412FA6B1C943CA05E38FD880E;
IL2CPP_EXTERN_C String_t* _stringLiteral4D1BDBFCC51F4695254B3C53B7142111EC52EA17;
IL2CPP_EXTERN_C String_t* _stringLiteral5176DB50285A90A97F8930F5CF3EA329C7B78385;
IL2CPP_EXTERN_C String_t* _stringLiteral51921D99887DD5ED233F87333EF648AE91A8BF7C;
IL2CPP_EXTERN_C String_t* _stringLiteral52C66E4B826647F480AEE3DD7DBC5996EC331410;
IL2CPP_EXTERN_C String_t* _stringLiteral549765DD842AF67AD155DB8B5FBFA4737CE548FC;
IL2CPP_EXTERN_C String_t* _stringLiteral558B14189B5200EFA26DCE8019A608B009BE50D4;
IL2CPP_EXTERN_C String_t* _stringLiteral55AC598ED5884D77F7D97920AA5DDDDA2CAA02B4;
IL2CPP_EXTERN_C String_t* _stringLiteral5850C06C770BC2AC6742A4BEF30C6D430FC07F2E;
IL2CPP_EXTERN_C String_t* _stringLiteral5A54AF115B00E5E16EBAE4AA0D0BC51A13317E05;
IL2CPP_EXTERN_C String_t* _stringLiteral5A854D0C545A398D8E4AAB887ACBEDCC9D9BDF41;
IL2CPP_EXTERN_C String_t* _stringLiteral5B0C5D37C82B62629FC978A74FD935409F9DCD8A;
IL2CPP_EXTERN_C String_t* _stringLiteral5B63100E80DAFEE5BA4AF4EDCCB7370ED6550264;
IL2CPP_EXTERN_C String_t* _stringLiteral5D0B7A1BC952F7F3CBB1B00E89E84090A2286942;
IL2CPP_EXTERN_C String_t* _stringLiteral5E89DFB28C21F05C783FF6C52C1E49EC5D97B20C;
IL2CPP_EXTERN_C String_t* _stringLiteral5E9D981D162913B37E33D18FF771488A4A34346E;
IL2CPP_EXTERN_C String_t* _stringLiteral65D8A29B27547180FAA9AF5295FC58F78CE0FEBD;
IL2CPP_EXTERN_C String_t* _stringLiteral65DCC08617A31D5E06A9612DBABB862DA1C4F062;
IL2CPP_EXTERN_C String_t* _stringLiteral66F9618FDA792CAB23AF2D7FFB50AB2D3E393DC5;
IL2CPP_EXTERN_C String_t* _stringLiteral678830C5836DCD590137DA23DA474CD589366649;
IL2CPP_EXTERN_C String_t* _stringLiteral6D79C4D8A34AE7B74EBCED100A562B73086EA268;
IL2CPP_EXTERN_C String_t* _stringLiteral6EA2D6BC4B09ED43616195D526B011F0DC643ECB;
IL2CPP_EXTERN_C String_t* _stringLiteral742AC0EDA1CCFC3576DC3F77C0A7F4EB0F9923C4;
IL2CPP_EXTERN_C String_t* _stringLiteral76BA0539B6046ABC30566265F439F0062F640CC0;
IL2CPP_EXTERN_C String_t* _stringLiteral77D2C3EA46482F5FF2F649EFEDC038D051B91098;
IL2CPP_EXTERN_C String_t* _stringLiteral7B8D306B8DAE57DC7D3095E04C9C6A2BFE3E7A4F;
IL2CPP_EXTERN_C String_t* _stringLiteral8032FA5FDE1CC0823EB09A003B765A5D49AB566C;
IL2CPP_EXTERN_C String_t* _stringLiteral80BFDED53A798895F66F7586BB93FB843218DB76;
IL2CPP_EXTERN_C String_t* _stringLiteral817EC3071B8F4D9C6EB59BDACEFB3E2CA37BD9F0;
IL2CPP_EXTERN_C String_t* _stringLiteral81861CA7BE722F39376AE14F09BA19F73DB86EBF;
IL2CPP_EXTERN_C String_t* _stringLiteral81B5C6B2E03435106B6CBAD2722668F8D5FC2913;
IL2CPP_EXTERN_C String_t* _stringLiteral8412159A618D7E89197A238D90096386B02C395E;
IL2CPP_EXTERN_C String_t* _stringLiteral84BD1476CFD81DB95A69E18C0BD3E1DE29BD872F;
IL2CPP_EXTERN_C String_t* _stringLiteral85169FA7FA3BE7DC7C81A7D284020DF267C61183;
IL2CPP_EXTERN_C String_t* _stringLiteral852638DF9819B10101A7347933AC592E1A7AF86E;
IL2CPP_EXTERN_C String_t* _stringLiteral85EA23745BAB27C1CF9C76837E55332314BA92E0;
IL2CPP_EXTERN_C String_t* _stringLiteral864CC40A200813B9284307874D1D3C8ACD06DE8C;
IL2CPP_EXTERN_C String_t* _stringLiteral88C9C0DAB7981E526880557EDF8F021E2F946C2D;
IL2CPP_EXTERN_C String_t* _stringLiteral897F348B17F4F3F52BB5A958E8F16003D3831485;
IL2CPP_EXTERN_C String_t* _stringLiteral8E752B76D455A50FE476984D4B09A7CDBF2A753E;
IL2CPP_EXTERN_C String_t* _stringLiteral8F113FA4A67982F9646FA7415A8AD19E9077A12A;
IL2CPP_EXTERN_C String_t* _stringLiteral9099FF4365E05FD8FF479542BF93799DD19F2D40;
IL2CPP_EXTERN_C String_t* _stringLiteral94107132E374EC913B72B94E192C66E8B0699CDA;
IL2CPP_EXTERN_C String_t* _stringLiteral98122CFEAFCE6941242F29CB3B619FAA1E78B828;
IL2CPP_EXTERN_C String_t* _stringLiteral9D7EFF3063C8C498DC4376D8A7C77CBD3894B949;
IL2CPP_EXTERN_C String_t* _stringLiteral9FFC6FBB48B64BB14E755F227D08648099006DA9;
IL2CPP_EXTERN_C String_t* _stringLiteralA1BD4986FCB7DB3734997A8A31846021B8C0D54E;
IL2CPP_EXTERN_C String_t* _stringLiteralA4419EF51FB63A77978E414E01AC1C9DCF20AA99;
IL2CPP_EXTERN_C String_t* _stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469;
IL2CPP_EXTERN_C String_t* _stringLiteralA45D1B7DECAFE0936897477A36E2DEEBDF9C830A;
IL2CPP_EXTERN_C String_t* _stringLiteralA493F2CAADD118807F8C20843D135AFAFCBBA254;
IL2CPP_EXTERN_C String_t* _stringLiteralA4E9B4490477EACED46B6D66D6EFFA258D8C3D7A;
IL2CPP_EXTERN_C String_t* _stringLiteralA50738BC6FDE22FAA8A8A82E2979A8FFA42C93A4;
IL2CPP_EXTERN_C String_t* _stringLiteralA51D1F1AA9592A683F8519425630E22AA5F35FD1;
IL2CPP_EXTERN_C String_t* _stringLiteralA65DD0DC43941EFA9FB37C63C1878CDEBE84FA20;
IL2CPP_EXTERN_C String_t* _stringLiteralA8C42238AB7DF269244EE10586CF4DE50612AAA3;
IL2CPP_EXTERN_C String_t* _stringLiteralAB3E708924BFB9D6B641A4B9F82FE5FE57F307B6;
IL2CPP_EXTERN_C String_t* _stringLiteralAEBD978DBA6DE5C36DCBB6A466CF1183121BCEA8;
IL2CPP_EXTERN_C String_t* _stringLiteralB521C0BA23951421DC8BB58874348C6E1EEC2E77;
IL2CPP_EXTERN_C String_t* _stringLiteralB599F7943E63846FF6287E29254EF871F7C11DD9;
IL2CPP_EXTERN_C String_t* _stringLiteralB7D525AABAD0632A9EC0EE33A7514F6C4892582B;
IL2CPP_EXTERN_C String_t* _stringLiteralB7E2E157CB850ECDEF7FC79B85F39918BAC5A8DD;
IL2CPP_EXTERN_C String_t* _stringLiteralB9E4F905776B9F5D49046F2E19FAF29798DB9E76;
IL2CPP_EXTERN_C String_t* _stringLiteralBC5DC870084E6ECC69F6896E17DF1CD0CC850F9A;
IL2CPP_EXTERN_C String_t* _stringLiteralBCAB68EDA292B4050A6B8D0864BDB3D03423FBB3;
IL2CPP_EXTERN_C String_t* _stringLiteralBCFD3E65D6CD9EF40881E41AFBC564269AC00026;
IL2CPP_EXTERN_C String_t* _stringLiteralBFB25F06F09F6041E2BDD623C059ACE65A0FB1DA;
IL2CPP_EXTERN_C String_t* _stringLiteralC0E2DE04AE40B3B0493F0F846F34B279C6D44FE9;
IL2CPP_EXTERN_C String_t* _stringLiteralC329109D65F880A37C0127AB93EAA77E304327AB;
IL2CPP_EXTERN_C String_t* _stringLiteralC3843AE7EC90FEDFFEE3510C1C5E1D6356927416;
IL2CPP_EXTERN_C String_t* _stringLiteralC611A012636D51B5EBBC7ADEBD3C8631EA8DAF13;
IL2CPP_EXTERN_C String_t* _stringLiteralC7B4D926EF9532A71B25AEC040A33D52C926425F;
IL2CPP_EXTERN_C String_t* _stringLiteralC972CCEF9725C97FA81EE0784238DBD804D49222;
IL2CPP_EXTERN_C String_t* _stringLiteralCA1511AE7356E5E0E5B6B5905112292E8DF67CB2;
IL2CPP_EXTERN_C String_t* _stringLiteralCA6526ECED6720495FFEC32D4D606B18F2C3A081;
IL2CPP_EXTERN_C String_t* _stringLiteralCB1BF9C3818A300118617C45409E803B828F1E9A;
IL2CPP_EXTERN_C String_t* _stringLiteralCC7BA483E7733CE41D9E3F9422E266682C65E9B2;
IL2CPP_EXTERN_C String_t* _stringLiteralCD002DD70C7AAC9CFF6D7D4821927E13D2989493;
IL2CPP_EXTERN_C String_t* _stringLiteralCE18B047107AA23D1AA9B2ED32D316148E02655F;
IL2CPP_EXTERN_C String_t* _stringLiteralD2D2F8D3F9F04A081FFBE6B2AF7917BAAADFC052;
IL2CPP_EXTERN_C String_t* _stringLiteralD4CCCB463309EDE01997B4A25530A4F9D64B0BAF;
IL2CPP_EXTERN_C String_t* _stringLiteralD520D5471D708F3D7ED6BA9DBE14564EB942076B;
IL2CPP_EXTERN_C String_t* _stringLiteralD559C6D97E819D8E4EF7ACDC34C4E8D3DD314964;
IL2CPP_EXTERN_C String_t* _stringLiteralDC51EFBBF1B5A672591C59CEF66A77386C61BE68;
IL2CPP_EXTERN_C String_t* _stringLiteralDE0657B16868C29C06BE2B1767DAD49310029B58;
IL2CPP_EXTERN_C String_t* _stringLiteralDED9BB89C032BA6D29ED4E9E2D13C150B7C43B2E;
IL2CPP_EXTERN_C String_t* _stringLiteralDF6F6437AE9DE592DED819C9D7AFC073AD780F19;
IL2CPP_EXTERN_C String_t* _stringLiteralE0B03E56BE7EFC27DFD5B413B4A5DD74DC0FB1DF;
IL2CPP_EXTERN_C String_t* _stringLiteralE3CFDB22842F54E3E0B119ACE3245306F0309BE0;
IL2CPP_EXTERN_C String_t* _stringLiteralE4F9B45B1B50EBC9511B1FDA35C85D4AD027C583;
IL2CPP_EXTERN_C String_t* _stringLiteralE7399AD4B42F540AF0F4AD91938F8F943B39424E;
IL2CPP_EXTERN_C String_t* _stringLiteralE79F08E3990626B5F3144E992D4D2F7D14584EC0;
IL2CPP_EXTERN_C String_t* _stringLiteralE8DE737330234B3EAC92FAE2AAB6B7DB5326CB91;
IL2CPP_EXTERN_C String_t* _stringLiteralE8FC4145DFB9A299846498E30E00134EB0ED6753;
IL2CPP_EXTERN_C String_t* _stringLiteralEB534843932D1025EEE09575458F840C63DC1063;
IL2CPP_EXTERN_C String_t* _stringLiteralEC79B3774CA39C0BF95A69EFBDEC79A32A5F3612;
IL2CPP_EXTERN_C String_t* _stringLiteralEC7F65B35829D8281484B259860DA096F9BAD474;
IL2CPP_EXTERN_C String_t* _stringLiteralF4A79D39206648981A640057F84D12739679216F;
IL2CPP_EXTERN_C String_t* _stringLiteralF51C0F9CE38D4A8832919DB2A9A19DC212BB790E;
IL2CPP_EXTERN_C String_t* _stringLiteralF628EB600A45B99D481CC2B1F52A62CC1B8169AF;
IL2CPP_EXTERN_C String_t* _stringLiteralF7F2EA94F25E42499DF1BBEA8E07B2BB10492332;
IL2CPP_EXTERN_C String_t* _stringLiteralF9010398F7F524C05AB19445BDCE02E617A3E267;
IL2CPP_EXTERN_C String_t* _stringLiteralFD4706B02823C71252FBF63A74CF03433A8DADF0;
IL2CPP_EXTERN_C String_t* _stringLiteralFF5C69F4E12AC1F06314CC73AC583302EE52390F;
IL2CPP_EXTERN_C const RuntimeMethod* AndroidFacebookGameObject_OnSceneLoaded_mE8FDAADE1E19AA7202CB27DC8C033676B52700BD_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* CallbackManager_AddFacebookDelegate_TisILoginResult_t809F9817555CF3FF1F2C11154D110DB3F55C07ED_m8D256EDC847AB71366C71A4DA1FA1B3A51E782AC_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* CanvasFacebook_OnGetAppLinkComplete_mA941FB6E6FC4EE30B35515478C5C5CC4CA2E8F64_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* CanvasFacebook_U3COnLoginCompleteU3Eb__37_0_mECC453F16A4ABDD1C5301986A3265C98AA4F3945_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* CanvasUIMethodCall_1__ctor_m09ADE50A6A66328C007485BF17C322DE3149D2CD_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* CanvasUIMethodCall_1__ctor_m1C9CF2D82C5B3A9F345A89045A43FF89270325F9_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* CanvasUIMethodCall_1__ctor_m3260CC061DE681C15F9551F21DBB0AB1914AD08D_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* ComponentFactory_GetComponent_TisAndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378_mAE5A1A01D902A79AC16E3656B097F207B8C4C9EC_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* ComponentFactory_GetComponent_TisCanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46_mE51621647A97AC742D31A41B486013594290ED7B_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* ComponentFactory_GetComponent_TisIOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA_m1537424BCE59DE26311F38D9FDC0B8F90AAEC688_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Dictionary_2_Add_m5875DF2ACE933D734119C088B2E7C9C63F49B443_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Dictionary_2_Add_mC78C20D5901C87AAC38F37C906FAB6946BDE5F13_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Dictionary_2__ctor_m768E076F1E804CE4959F4E71D3E6A9ADE2F55052_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Dictionary_2__ctor_mC4F3DF292BAD88F4BF193C49CD689FAEBC4570A9_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Enumerable_Any_TisRuntimeObject_m67CFBD544CF1D1C0C7E7457FDBDB81649DE26847_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Enumerable_First_TisRuntimeObject_mEFECF1B8C3201589C5AF34176DCBF8DD926642D6_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Enumerable_ToDictionary_TisKeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A_TisString_t_TisRuntimeObject_m1565E67B7F63889DEE95110A76BCB790092DD21B_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* GameObject_AddComponent_TisJsBridge_t338341DF8272C935803F827012749337260971DA_mE386CD7945F07943362FF25193BDB880EAFE1AE3_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* IAndroidWrapper_CallStatic_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_m853C4A273DD838FD248C6D7A55AFEAE879A39499_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* IAndroidWrapper_CallStatic_TisString_t_m53CBBE3DF66F9F471C84A81702233C750461949A_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m07934BE9727E716B67132D5648DCC2FD307D5281_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m1A91A6D8302126249B270F478B851AE405105FEE_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m23975F7FD0FE3A43155D75E53A95085F6504F9CD_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m25A837E2CB92362A09A93D1260F3D010966A513E_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m550304F60083DC63749F387B03DF9E7777F83265_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m5F7216810B33A4CCED764468B20F505B5449669D_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m633D3D2232A91D7FBE7FA9F6E87212EC1F47708C_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m82480001BC5DA6A73E94BC8C41F4296473A9141F_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m88BEF0F791A57E9BAF9D3C26D3053BBD54F4BD8D_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m98326A81B2D27D3EF9182B178B0B3B026C6A41E6_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_m9EDA60F8B74B5DE2F1D9781C1D09AE56F5F490FA_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mA93CFE9895EE4033CEE85613734423E1E8870310_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mAC2E2E15420B915144E3A7324B6F901123C5BC22_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mBC1E3309CE7ADE92E369530993D7A36F5EFEF649_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mC2486364FEAA45691A130BAFFF533BC1384E5BEF_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mCD10E054583159BC1950612666A16FD33761CBE4_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mCD8FA794A4409286C7AFB41CA358BD8620A1D230_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mD281D9CF6AA50E4AF6D86C8046B972E4759C82E0_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mEDBCDC346A8A1302DA0A03038DF6159D0DA5FB9E_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mF266633C897B9D87553E0CB9CF81EAE049A9A423_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* JavaMethodCall_1__ctor_mF67A248E040456EE62DDE37D0E7A8BE10AFFD665_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* KeyValuePair_2_get_Key_m654BCCAE2F20CB11D8E8C2D2C886A0C8A13EB1C4_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* KeyValuePair_2_get_Key_mA64FF29A08423140758B0276333D1A89C71B793A_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* KeyValuePair_2_get_Value_m2052BF44A3FDE623D98B0E6B6E227B2900034235_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* KeyValuePair_2_get_Value_m7345512A32CB4DCAA0643050B18DC8DCD71B927A_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* List_1__ctor_mCA8DD57EAC70C2B5923DBB9D5A77CEAC22E7068E_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodArguments_AddList_TisRuntimeObject_mD72031331AC5437B40C6D0570A060B9271CFF369_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodArguments_AddList_TisString_t_mBBEF6565354D6AE11E537FC132BC332546C22099_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodArguments_AddNullablePrimitive_TisOGActionType_t779FA0C877B3CD5177F27123B2E9CB4CEB14EC2E_mF55BAB58959DD27895825BFF8DE9DCEEAC13DED7_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m00241290CE194EF41B420201C9BC57834F005704_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m0300E7B325CFF8E837D2D5B77E3B160C53C1068F_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m097F0A862D0980C829D48D394888D0370C9EFB85_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m1725F4044DF470B54304044677371FA010FCA055_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m2FE69B0E3B56B162D1F585BF08320F54F3EB73CB_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m31B32945ADD64F439F695CD2B613934BEFCC15A8_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m33FD8977CC0AFF79A6EBA5E2DE85109BDB68ED8F_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m37167CE00F34CC6A9A24918CB34160CB9ECA017E_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m3AC34DDE1F2C7F926E3440F251C1519997E656B5_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m490C4FB7E8194080AD5A53C8BA7F96134C1EED27_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m4EB4144CB0D6BAAD051358C0C21EA8D28E43A4DB_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m5ACC9D63CAC86405A106A94B35C11B8641FA2088_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m695883E994C52B5CB6C0AB3939ED902B2F1455A5_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m6AE07A293D1DB63C07A09D30AC74C42874098633_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m754DB9950904B1CCB5076A82982D93946AF22BE3_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m7B3DF88D41AC203597412EF880B1442EA25112B4_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m7C7EFBA28D827AB9DEE7946EBD20C87AD1EB44C5_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_m80D0B98B0FF86073D4DE52A2FB8C56374851424A_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_mCE2EA397F7DFB68974A15A329A01050C09E92A61_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_mE524DA942B4350B09ECDF58D3FA0D6D3765CAE4D_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* MethodCall_1_set_Callback_mFFB10B41B3F6834AAF0A0A2D1EB306A89AA8E4A1_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Nullable_1_GetValueOrDefault_m068A148705ED1E215A5E85D18BA6852B192DA419_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Nullable_1_ToString_mF981686677572249978468566375A4C296C6B97A_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Nullable_1_get_HasValue_m3C0F9BCB83ED49443257921B53C3AC3A95FEDC63_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Nullable_1_get_HasValue_mC149B1C717AF506BBE8932F2C1DC86C378D17EA8_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* U3CU3Ec_U3CCreateTournamentU3Eb__65_0_mFA14522DDB5C630904E8C38ABBD1D9345954954B_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* U3CU3Ec_U3CCreateTournamentU3Eb__65_1_mE9A32D06A70E684E33F841470131880439A05509_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* U3CU3Ec_U3COnFacebookAuthResponseChangeU3Eb__40_0_m6BDA2E988B1B4F39B882D8A24DE94F228F1B481F_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* U3CU3Ec_U3CShareTournamentU3Eb__66_0_mA62839F14645C8D8049BAD3091FA31EFE8C31ACE_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* U3CU3Ec_U3CShareTournamentU3Eb__66_1_m86A8DF73410D88DEE63BE781B5D30F7E3240D92A_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* U3CU3Ec__DisplayClass47_0_U3CFormatAuthResponseU3Eb__0_m601241C800917FCB891C39BCCEA9988624FDE94A_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Utilities_TryGetValue_TisIDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_mF4F1612A6CC71E09B0A940E7B840C94637D8BB97_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Utilities_TryGetValue_TisIList_1_t6EE90D273EFCF5E7E4C37FAB712E70BB6F1B4BFF_mC232E1497C9BEC609103DB1B2BE3EDF69B141C16_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Utilities_TryGetValue_TisString_t_m2341652CD88C1BF40208CFE6D13B0EF59437D8CC_RuntimeMethod_var;
struct CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D_marshaled_com;
struct CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D_marshaled_pinvoke;
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_com;
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_pinvoke;
struct Delegate_t_marshaled_com;
struct Delegate_t_marshaled_pinvoke;
struct Exception_t_marshaled_com;
struct Exception_t_marshaled_pinvoke;

struct CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB;
struct ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918;
struct StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248;

IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.Collections.Generic.Dictionary`2<System.String,System.Object>
struct Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710  : public RuntimeObject
{
	// System.Int32[] System.Collections.Generic.Dictionary`2::_buckets
	Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* ____buckets_0;
	// System.Collections.Generic.Dictionary`2/Entry<TKey,TValue>[] System.Collections.Generic.Dictionary`2::_entries
	EntryU5BU5D_t233BB24ED01E2D8D65B0651D54B8E3AD125CAF96* ____entries_1;
	// System.Int32 System.Collections.Generic.Dictionary`2::_count
	int32_t ____count_2;
	// System.Int32 System.Collections.Generic.Dictionary`2::_freeList
	int32_t ____freeList_3;
	// System.Int32 System.Collections.Generic.Dictionary`2::_freeCount
	int32_t ____freeCount_4;
	// System.Int32 System.Collections.Generic.Dictionary`2::_version
	int32_t ____version_5;
	// System.Collections.Generic.IEqualityComparer`1<TKey> System.Collections.Generic.Dictionary`2::_comparer
	RuntimeObject* ____comparer_6;
	// System.Collections.Generic.Dictionary`2/KeyCollection<TKey,TValue> System.Collections.Generic.Dictionary`2::_keys
	KeyCollection_tE66790F09E854C19C7F612BEAD203AE626E90A36* ____keys_7;
	// System.Collections.Generic.Dictionary`2/ValueCollection<TKey,TValue> System.Collections.Generic.Dictionary`2::_values
	ValueCollection_tC9D91E8A3198E40EA339059703AB10DFC9F5CC2E* ____values_8;
	// System.Object System.Collections.Generic.Dictionary`2::_syncRoot
	RuntimeObject* ____syncRoot_9;
};

// System.Collections.Generic.Dictionary`2<System.String,System.String>
struct Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83  : public RuntimeObject
{
	// System.Int32[] System.Collections.Generic.Dictionary`2::_buckets
	Int32U5BU5D_t19C97395396A72ECAF310612F0760F165060314C* ____buckets_0;
	// System.Collections.Generic.Dictionary`2/Entry<TKey,TValue>[] System.Collections.Generic.Dictionary`2::_entries
	EntryU5BU5D_t1AF33AD0B7330843448956EC4277517081658AE7* ____entries_1;
	// System.Int32 System.Collections.Generic.Dictionary`2::_count
	int32_t ____count_2;
	// System.Int32 System.Collections.Generic.Dictionary`2::_freeList
	int32_t ____freeList_3;
	// System.Int32 System.Collections.Generic.Dictionary`2::_freeCount
	int32_t ____freeCount_4;
	// System.Int32 System.Collections.Generic.Dictionary`2::_version
	int32_t ____version_5;
	// System.Collections.Generic.IEqualityComparer`1<TKey> System.Collections.Generic.Dictionary`2::_comparer
	RuntimeObject* ____comparer_6;
	// System.Collections.Generic.Dictionary`2/KeyCollection<TKey,TValue> System.Collections.Generic.Dictionary`2::_keys
	KeyCollection_t2EDD317F5771E575ACB63527B5AFB71291040342* ____keys_7;
	// System.Collections.Generic.Dictionary`2/ValueCollection<TKey,TValue> System.Collections.Generic.Dictionary`2::_values
	ValueCollection_t238D0D2427C6B841A01F522A41540165A2C4AE76* ____values_8;
	// System.Object System.Collections.Generic.Dictionary`2::_syncRoot
	RuntimeObject* ____syncRoot_9;
};

// System.Collections.Generic.List`1<System.String>
struct List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD  : public RuntimeObject
{
	// T[] System.Collections.Generic.List`1::_items
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ____items_1;
	// System.Int32 System.Collections.Generic.List`1::_size
	int32_t ____size_2;
	// System.Int32 System.Collections.Generic.List`1::_version
	int32_t ____version_3;
	// System.Object System.Collections.Generic.List`1::_syncRoot
	RuntimeObject* ____syncRoot_4;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>
struct MethodCall_1_t1852FDCA7708C74BD9575219E6FC7F609E56BA8E  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t0A787A64D3187E98A865A198DDF444C008C79F35* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IAppLinkResult>
struct MethodCall_1_t73261478565B64ABA19B127D15BD7DCCD81D6A06  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IAppRequestResult>
struct MethodCall_1_t1D2B62ADEB209FD37D3B0AED0788B5A36C325DED  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.ICatalogResult>
struct MethodCall_1_tA51B63B488D06F9DD4269E5D936844297AC2F717  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_tA8F51BBA2E7364881368E0CBED892F561162C32E* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IConsumePurchaseResult>
struct MethodCall_1_t170D11D6663ABDBA2714482029BC9107E6119F32  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_tF96C838D4977CEFDB874E77F7A56B1CFECF4D8DD* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>
struct MethodCall_1_t28181D9D5D0204C879A034D9757AF1F63C3EC7AF  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t55619BCDC758CB1C82277E6D8BA44ACA47C03DE2* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IIAPReadyResult>
struct MethodCall_1_t3129C45AEAB38F50384EF247D7ED9921171D1DF7  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_tA2A619ACCF137D8094955A556E78A66749C43F74* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IInitCloudGameResult>
struct MethodCall_1_tFDD6DDBFBE6A9432424C4841D30F5018A40CFEDC  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_tB829CDA8266CAB15D1703F5904BB2F8592CA60E8* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IInterstitialAdResult>
struct MethodCall_1_t7E8B67FDC468ABDD86D3E481B50034289CB7279D  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t6BAE034F2CC3270BFC282A9D0DEB58CC5F91C265* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.ILoginResult>
struct MethodCall_1_tFC828FE9EEAFBD29ACB6560A77095DF9CC02959F  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IMediaUploadResult>
struct MethodCall_1_tACE924324819B8E5DAC6615D0A058E16C5A7C729  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t7CA00F6A27B3FE85139590EFEA03B7C7C0D4A66D* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IOpenAppStoreResult>
struct MethodCall_1_t87E54F439C1534715A53A604C0EAE5850B55120D  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t5E457BF6B03D4AD72D97F1848A0612B675747CFD* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPayResult>
struct MethodCall_1_t222223E3C8CAEF78C55722F5474C0C2C66F0EA2B  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPayloadResult>
struct MethodCall_1_tDD7FB7C96BB6551BC13B0499833326C4B55CB468  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_tB6B04FA63685B9A0D41AB08E3E5BC96A72D7A1D7* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchaseResult>
struct MethodCall_1_tE2E7E098CBA48E270C26249E671C3D59EB7D0C27  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_tBD274D4AED39A6A24A22981D38D3CBF447CF036E* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchasesResult>
struct MethodCall_1_t964EEAC4E9A2090141E286A38DD20AD5D31C42B3  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t61D056F5D405CB3DF2F643449D9C03A64AB8E818* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IResult>
struct MethodCall_1_tDBF0393A25B67ED61959BD6A2ED2ED86222FF7F4  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_tD24CEC09066861D3AC22EC3D0CFC1542F4592B1D* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IRewardedVideoResult>
struct MethodCall_1_tDFA97C21B6B2B2BC255F28E227D0F5FA5829F686  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t9C5748E8AC36242F3F03171AA1E6306E60860ACD* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>
struct MethodCall_1_t619F8AD0F81AEF389F5C6063A4C1F8C06036B029  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t623E520D5B99523D410AEE72AB9FF4901DC356AC* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.ISessionScoreResult>
struct MethodCall_1_t3A25163740B62335265043F26060847249D6C047  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t610D359C8E47669CC0D9F0B6CA9DF9240BF80267* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.IShareResult>
struct MethodCall_1_t0D27E61246A32F94E6F80F665C505FC54279576C  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentResult>
struct MethodCall_1_tAE6A4425ED88803929C6E6458A9EF1A83135B315  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_tD9C9BBFE6AD0931C935E391D858C37BEBF14BBA3* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentScoreResult>
struct MethodCall_1_tACF312248F1E681E1DA6AF486A149C37756A81E3  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_t474682078C474498C8D4805F13E9077763B39255* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// Facebook.Unity.MethodCall`1<System.Object>
struct MethodCall_1_tBE309A8BE882A6A1F80E42A8EE1493B9CB4FBBC8  : public RuntimeObject
{
	// System.String Facebook.Unity.MethodCall`1::<MethodName>k__BackingField
	String_t* ___U3CMethodNameU3Ek__BackingField_0;
	// Facebook.Unity.FacebookDelegate`1<T> Facebook.Unity.MethodCall`1::<Callback>k__BackingField
	FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9* ___U3CCallbackU3Ek__BackingField_1;
	// Facebook.Unity.FacebookBase Facebook.Unity.MethodCall`1::<FacebookImpl>k__BackingField
	FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* ___U3CFacebookImplU3Ek__BackingField_2;
	// Facebook.Unity.MethodArguments Facebook.Unity.MethodCall`1::<Parameters>k__BackingField
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* ___U3CParametersU3Ek__BackingField_3;
};

// System.Reflection.Assembly
struct Assembly_t  : public RuntimeObject
{
};
// Native definition for P/Invoke marshalling of System.Reflection.Assembly
struct Assembly_t_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.Reflection.Assembly
struct Assembly_t_marshaled_com
{
};

// Facebook.Unity.AuthenticationToken
struct AuthenticationToken_t925F4C42BE7D08897D579F0A55D5682704237B8C  : public RuntimeObject
{
	// System.String Facebook.Unity.AuthenticationToken::<TokenString>k__BackingField
	String_t* ___U3CTokenStringU3Ek__BackingField_0;
	// System.String Facebook.Unity.AuthenticationToken::<Nonce>k__BackingField
	String_t* ___U3CNonceU3Ek__BackingField_1;
};

// Facebook.Unity.CallbackManager
struct CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749  : public RuntimeObject
{
	// System.Collections.Generic.IDictionary`2<System.String,System.Object> Facebook.Unity.CallbackManager::facebookDelegates
	RuntimeObject* ___facebookDelegates_0;
	// System.Int32 Facebook.Unity.CallbackManager::nextAsyncId
	int32_t ___nextAsyncId_1;
};

// System.Globalization.CultureInfo
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0  : public RuntimeObject
{
	// System.Boolean System.Globalization.CultureInfo::m_isReadOnly
	bool ___m_isReadOnly_3;
	// System.Int32 System.Globalization.CultureInfo::cultureID
	int32_t ___cultureID_4;
	// System.Int32 System.Globalization.CultureInfo::parent_lcid
	int32_t ___parent_lcid_5;
	// System.Int32 System.Globalization.CultureInfo::datetime_index
	int32_t ___datetime_index_6;
	// System.Int32 System.Globalization.CultureInfo::number_index
	int32_t ___number_index_7;
	// System.Int32 System.Globalization.CultureInfo::default_calendar_type
	int32_t ___default_calendar_type_8;
	// System.Boolean System.Globalization.CultureInfo::m_useUserOverride
	bool ___m_useUserOverride_9;
	// System.Globalization.NumberFormatInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::numInfo
	NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* ___numInfo_10;
	// System.Globalization.DateTimeFormatInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::dateTimeInfo
	DateTimeFormatInfo_t0457520F9FA7B5C8EAAEB3AD50413B6AEEB7458A* ___dateTimeInfo_11;
	// System.Globalization.TextInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::textInfo
	TextInfo_tD3BAFCFD77418851E7D5CB8D2588F47019E414B4* ___textInfo_12;
	// System.String System.Globalization.CultureInfo::m_name
	String_t* ___m_name_13;
	// System.String System.Globalization.CultureInfo::englishname
	String_t* ___englishname_14;
	// System.String System.Globalization.CultureInfo::nativename
	String_t* ___nativename_15;
	// System.String System.Globalization.CultureInfo::iso3lang
	String_t* ___iso3lang_16;
	// System.String System.Globalization.CultureInfo::iso2lang
	String_t* ___iso2lang_17;
	// System.String System.Globalization.CultureInfo::win3lang
	String_t* ___win3lang_18;
	// System.String System.Globalization.CultureInfo::territory
	String_t* ___territory_19;
	// System.String[] System.Globalization.CultureInfo::native_calendar_names
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___native_calendar_names_20;
	// System.Globalization.CompareInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::compareInfo
	CompareInfo_t1B1A6AC3486B570C76ABA52149C9BD4CD82F9E57* ___compareInfo_21;
	// System.Void* System.Globalization.CultureInfo::textinfo_data
	void* ___textinfo_data_22;
	// System.Int32 System.Globalization.CultureInfo::m_dataItem
	int32_t ___m_dataItem_23;
	// System.Globalization.Calendar System.Globalization.CultureInfo::calendar
	Calendar_t0A117CC7532A54C17188C2EFEA1F79DB20DF3A3B* ___calendar_24;
	// System.Globalization.CultureInfo System.Globalization.CultureInfo::parent_culture
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___parent_culture_25;
	// System.Boolean System.Globalization.CultureInfo::constructed
	bool ___constructed_26;
	// System.Byte[] System.Globalization.CultureInfo::cached_serialized_form
	ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* ___cached_serialized_form_27;
	// System.Globalization.CultureData System.Globalization.CultureInfo::m_cultureData
	CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D* ___m_cultureData_28;
	// System.Boolean System.Globalization.CultureInfo::m_isInherited
	bool ___m_isInherited_29;
};
// Native definition for P/Invoke marshalling of System.Globalization.CultureInfo
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_pinvoke
{
	int32_t ___m_isReadOnly_3;
	int32_t ___cultureID_4;
	int32_t ___parent_lcid_5;
	int32_t ___datetime_index_6;
	int32_t ___number_index_7;
	int32_t ___default_calendar_type_8;
	int32_t ___m_useUserOverride_9;
	NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* ___numInfo_10;
	DateTimeFormatInfo_t0457520F9FA7B5C8EAAEB3AD50413B6AEEB7458A* ___dateTimeInfo_11;
	TextInfo_tD3BAFCFD77418851E7D5CB8D2588F47019E414B4* ___textInfo_12;
	char* ___m_name_13;
	char* ___englishname_14;
	char* ___nativename_15;
	char* ___iso3lang_16;
	char* ___iso2lang_17;
	char* ___win3lang_18;
	char* ___territory_19;
	char** ___native_calendar_names_20;
	CompareInfo_t1B1A6AC3486B570C76ABA52149C9BD4CD82F9E57* ___compareInfo_21;
	void* ___textinfo_data_22;
	int32_t ___m_dataItem_23;
	Calendar_t0A117CC7532A54C17188C2EFEA1F79DB20DF3A3B* ___calendar_24;
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_pinvoke* ___parent_culture_25;
	int32_t ___constructed_26;
	Il2CppSafeArray/*NONE*/* ___cached_serialized_form_27;
	CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D_marshaled_pinvoke* ___m_cultureData_28;
	int32_t ___m_isInherited_29;
};
// Native definition for COM marshalling of System.Globalization.CultureInfo
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_com
{
	int32_t ___m_isReadOnly_3;
	int32_t ___cultureID_4;
	int32_t ___parent_lcid_5;
	int32_t ___datetime_index_6;
	int32_t ___number_index_7;
	int32_t ___default_calendar_type_8;
	int32_t ___m_useUserOverride_9;
	NumberFormatInfo_t8E26808B202927FEBF9064FCFEEA4D6E076E6472* ___numInfo_10;
	DateTimeFormatInfo_t0457520F9FA7B5C8EAAEB3AD50413B6AEEB7458A* ___dateTimeInfo_11;
	TextInfo_tD3BAFCFD77418851E7D5CB8D2588F47019E414B4* ___textInfo_12;
	Il2CppChar* ___m_name_13;
	Il2CppChar* ___englishname_14;
	Il2CppChar* ___nativename_15;
	Il2CppChar* ___iso3lang_16;
	Il2CppChar* ___iso2lang_17;
	Il2CppChar* ___win3lang_18;
	Il2CppChar* ___territory_19;
	Il2CppChar** ___native_calendar_names_20;
	CompareInfo_t1B1A6AC3486B570C76ABA52149C9BD4CD82F9E57* ___compareInfo_21;
	void* ___textinfo_data_22;
	int32_t ___m_dataItem_23;
	Calendar_t0A117CC7532A54C17188C2EFEA1F79DB20DF3A3B* ___calendar_24;
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_marshaled_com* ___parent_culture_25;
	int32_t ___constructed_26;
	Il2CppSafeArray/*NONE*/* ___cached_serialized_form_27;
	CultureData_tEEFDCF4ECA1BBF6C0C8C94EB3541657245598F9D_marshaled_com* ___m_cultureData_28;
	int32_t ___m_isInherited_29;
};

// Facebook.Unity.FBLocation
struct FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236  : public RuntimeObject
{
	// System.String Facebook.Unity.FBLocation::<ID>k__BackingField
	String_t* ___U3CIDU3Ek__BackingField_0;
	// System.String Facebook.Unity.FBLocation::<Name>k__BackingField
	String_t* ___U3CNameU3Ek__BackingField_1;
};

// Facebook.Unity.FacebookBase
struct FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865  : public RuntimeObject
{
	// Facebook.Unity.InitDelegate Facebook.Unity.FacebookBase::onInitCompleteDelegate
	InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F* ___onInitCompleteDelegate_0;
	// System.Boolean Facebook.Unity.FacebookBase::<Initialized>k__BackingField
	bool ___U3CInitializedU3Ek__BackingField_1;
	// Facebook.Unity.CallbackManager Facebook.Unity.FacebookBase::<CallbackManager>k__BackingField
	CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* ___U3CCallbackManagerU3Ek__BackingField_2;
};

// System.Reflection.MemberInfo
struct MemberInfo_t  : public RuntimeObject
{
};

// Facebook.Unity.MethodArguments
struct MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD  : public RuntimeObject
{
	// System.Collections.Generic.IDictionary`2<System.String,System.Object> Facebook.Unity.MethodArguments::arguments
	RuntimeObject* ___arguments_0;
};

// Facebook.Unity.ResultContainer
struct ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B  : public RuntimeObject
{
	// System.String Facebook.Unity.ResultContainer::<RawResult>k__BackingField
	String_t* ___U3CRawResultU3Ek__BackingField_1;
	// System.Collections.Generic.IDictionary`2<System.String,System.Object> Facebook.Unity.ResultContainer::<ResultDictionary>k__BackingField
	RuntimeObject* ___U3CResultDictionaryU3Ek__BackingField_2;
};

// System.String
struct String_t  : public RuntimeObject
{
	// System.Int32 System.String::_stringLength
	int32_t ____stringLength_4;
	// System.Char System.String::_firstChar
	Il2CppChar ____firstChar_5;
};

// System.Uri
struct Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E  : public RuntimeObject
{
	// System.String System.Uri::m_String
	String_t* ___m_String_16;
	// System.String System.Uri::m_originalUnicodeString
	String_t* ___m_originalUnicodeString_17;
	// System.UriParser System.Uri::m_Syntax
	UriParser_t920B0868286118827C08B08A15A9456AF6C19D81* ___m_Syntax_18;
	// System.String System.Uri::m_DnsSafeHost
	String_t* ___m_DnsSafeHost_19;
	// System.Uri/Flags System.Uri::m_Flags
	uint64_t ___m_Flags_20;
	// System.Uri/UriInfo System.Uri::m_Info
	UriInfo_t5F91F77A93545DDDA6BB24A609BAF5E232CC1A09* ___m_Info_21;
	// System.Boolean System.Uri::m_iriParsing
	bool ___m_iriParsing_22;
};

// Facebook.Unity.UserAgeRange
struct UserAgeRange_t3074872BAC5CB213D6E7245C5C9407CA12B7321A  : public RuntimeObject
{
	// System.Int64 Facebook.Unity.UserAgeRange::<Min>k__BackingField
	int64_t ___U3CMinU3Ek__BackingField_0;
	// System.Int64 Facebook.Unity.UserAgeRange::<Max>k__BackingField
	int64_t ___U3CMaxU3Ek__BackingField_1;
};

// System.ValueType
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F  : public RuntimeObject
{
};
// Native definition for P/Invoke marshalling of System.ValueType
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.ValueType
struct ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F_marshaled_com
{
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/<>c
struct U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD  : public RuntimeObject
{
};

// Facebook.Unity.Canvas.CanvasFacebook/<>c
struct U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8  : public RuntimeObject
{
};

// Facebook.Unity.Canvas.CanvasFacebook/<>c__DisplayClass47_0
struct U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A  : public RuntimeObject
{
	// Facebook.Unity.ResultContainer Facebook.Unity.Canvas.CanvasFacebook/<>c__DisplayClass47_0::result
	ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___result_0;
	// Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer> Facebook.Unity.Canvas.CanvasFacebook/<>c__DisplayClass47_0::callback
	Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* ___callback_1;
};

// Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict
struct NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9  : public RuntimeObject
{
	// System.Int32 Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::<NumEntries>k__BackingField
	int32_t ___U3CNumEntriesU3Ek__BackingField_0;
	// System.String[] Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::<Keys>k__BackingField
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___U3CKeysU3Ek__BackingField_1;
	// System.String[] Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::<Values>k__BackingField
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___U3CValuesU3Ek__BackingField_2;
};

// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IAppRequestResult>
struct CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE  : public MethodCall_1_t1D2B62ADEB209FD37D3B0AED0788B5A36C325DED
{
	// Facebook.Unity.Canvas.CanvasFacebook Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1::canvasImpl
	CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* ___canvasImpl_4;
	// System.String Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1::callbackMethod
	String_t* ___callbackMethod_5;
};

// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IPayResult>
struct CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6  : public MethodCall_1_t222223E3C8CAEF78C55722F5474C0C2C66F0EA2B
{
	// Facebook.Unity.Canvas.CanvasFacebook Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1::canvasImpl
	CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* ___canvasImpl_4;
	// System.String Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1::callbackMethod
	String_t* ___callbackMethod_5;
};

// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IShareResult>
struct CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052  : public MethodCall_1_t0D27E61246A32F94E6F80F665C505FC54279576C
{
	// Facebook.Unity.Canvas.CanvasFacebook Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1::canvasImpl
	CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* ___canvasImpl_4;
	// System.String Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1::callbackMethod
	String_t* ___callbackMethod_5;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>
struct JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F  : public MethodCall_1_t1852FDCA7708C74BD9575219E6FC7F609E56BA8E
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppLinkResult>
struct JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B  : public MethodCall_1_t73261478565B64ABA19B127D15BD7DCCD81D6A06
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppRequestResult>
struct JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1  : public MethodCall_1_t1D2B62ADEB209FD37D3B0AED0788B5A36C325DED
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ICatalogResult>
struct JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A  : public MethodCall_1_tA51B63B488D06F9DD4269E5D936844297AC2F717
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IConsumePurchaseResult>
struct JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB  : public MethodCall_1_t170D11D6663ABDBA2714482029BC9107E6119F32
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>
struct JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524  : public MethodCall_1_t28181D9D5D0204C879A034D9757AF1F63C3EC7AF
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IIAPReadyResult>
struct JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53  : public MethodCall_1_t3129C45AEAB38F50384EF247D7ED9921171D1DF7
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInitCloudGameResult>
struct JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8  : public MethodCall_1_tFDD6DDBFBE6A9432424C4841D30F5018A40CFEDC
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInterstitialAdResult>
struct JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466  : public MethodCall_1_t7E8B67FDC468ABDD86D3E481B50034289CB7279D
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ILoginResult>
struct JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96  : public MethodCall_1_tFC828FE9EEAFBD29ACB6560A77095DF9CC02959F
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IMediaUploadResult>
struct JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA  : public MethodCall_1_tACE924324819B8E5DAC6615D0A058E16C5A7C729
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IOpenAppStoreResult>
struct JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5  : public MethodCall_1_t87E54F439C1534715A53A604C0EAE5850B55120D
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPayloadResult>
struct JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7  : public MethodCall_1_tDD7FB7C96BB6551BC13B0499833326C4B55CB468
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchaseResult>
struct JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7  : public MethodCall_1_tE2E7E098CBA48E270C26249E671C3D59EB7D0C27
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchasesResult>
struct JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D  : public MethodCall_1_t964EEAC4E9A2090141E286A38DD20AD5D31C42B3
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IResult>
struct JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26  : public MethodCall_1_tDBF0393A25B67ED61959BD6A2ED2ED86222FF7F4
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IRewardedVideoResult>
struct JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20  : public MethodCall_1_tDFA97C21B6B2B2BC255F28E227D0F5FA5829F686
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>
struct JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152  : public MethodCall_1_t619F8AD0F81AEF389F5C6063A4C1F8C06036B029
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ISessionScoreResult>
struct JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862  : public MethodCall_1_t3A25163740B62335265043F26060847249D6C047
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IShareResult>
struct JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3  : public MethodCall_1_t0D27E61246A32F94E6F80F665C505FC54279576C
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentResult>
struct JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71  : public MethodCall_1_tAE6A4425ED88803929C6E6458A9EF1A83135B315
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentScoreResult>
struct JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557  : public MethodCall_1_tACF312248F1E681E1DA6AF486A149C37756A81E3
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1::androidImpl
	AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___androidImpl_4;
};

// System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>
struct KeyValuePair_2_tFC32D2507216293851350D29B64D79F950B55230 
{
	// TKey System.Collections.Generic.KeyValuePair`2::key
	RuntimeObject* ___key_0;
	// TValue System.Collections.Generic.KeyValuePair`2::value
	RuntimeObject* ___value_1;
};

// System.Collections.Generic.KeyValuePair`2<System.String,System.Object>
struct KeyValuePair_2_tBEE55F2A4574C64393155C322376FD98C7BFC7B9 
{
	// TKey System.Collections.Generic.KeyValuePair`2::key
	String_t* ___key_0;
	// TValue System.Collections.Generic.KeyValuePair`2::value
	RuntimeObject* ___value_1;
};

// System.Collections.Generic.KeyValuePair`2<System.String,System.String>
struct KeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A 
{
	// TKey System.Collections.Generic.KeyValuePair`2::key
	String_t* ___key_0;
	// TValue System.Collections.Generic.KeyValuePair`2::value
	String_t* ___value_1;
};

// System.Nullable`1<System.Int32>
struct Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 
{
	// System.Boolean System.Nullable`1::hasValue
	bool ___hasValue_0;
	// T System.Nullable`1::value
	int32_t ___value_1;
};

// System.Nullable`1<System.Int32Enum>
struct Nullable_1_t163D49A1147F217B7BD43BE8ACC8A5CC6B846D14 
{
	// System.Boolean System.Nullable`1::hasValue
	bool ___hasValue_0;
	// T System.Nullable`1::value
	int32_t ___value_1;
};

// System.Nullable`1<System.Int64>
struct Nullable_1_t365991B3904FDA7642A788423B28692FDC7CDB17 
{
	// System.Boolean System.Nullable`1::hasValue
	bool ___hasValue_0;
	// T System.Nullable`1::value
	int64_t ___value_1;
};

// System.Nullable`1<Facebook.Unity.OGActionType>
struct Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73 
{
	// System.Boolean System.Nullable`1::hasValue
	bool ___hasValue_0;
	// T System.Nullable`1::value
	int32_t ___value_1;
};

// System.Nullable`1<System.Single>
struct Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75 
{
	// System.Boolean System.Nullable`1::hasValue
	bool ___hasValue_0;
	// T System.Nullable`1::value
	float ___value_1;
};

// System.Boolean
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	// System.Boolean System.Boolean::m_value
	bool ___m_value_0;
};

// Facebook.Unity.Canvas.CanvasFacebook
struct CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1  : public FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865
{
	// System.String Facebook.Unity.Canvas.CanvasFacebook::appId
	String_t* ___appId_3;
	// System.String Facebook.Unity.Canvas.CanvasFacebook::appLinkUrl
	String_t* ___appLinkUrl_4;
	// Facebook.Unity.Canvas.ICanvasJSWrapper Facebook.Unity.Canvas.CanvasFacebook::canvasJSWrapper
	RuntimeObject* ___canvasJSWrapper_5;
	// Facebook.Unity.HideUnityDelegate Facebook.Unity.Canvas.CanvasFacebook::onHideUnityDelegate
	HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE* ___onHideUnityDelegate_6;
	// System.Boolean Facebook.Unity.Canvas.CanvasFacebook::<LimitEventUsage>k__BackingField
	bool ___U3CLimitEventUsageU3Ek__BackingField_7;
};

// System.Char
struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17 
{
	// System.Char System.Char::m_value
	Il2CppChar ___m_value_0;
};

// System.DateTime
struct DateTime_t66193957C73913903DDAD89FEDC46139BCA5802D 
{
	// System.UInt64 System.DateTime::_dateData
	uint64_t ____dateData_46;
};

// System.Enum
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2  : public ValueType_t6D9B272BD21782F0A9A14F2E41F85A50E97A986F
{
};
// Native definition for P/Invoke marshalling of System.Enum
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2_marshaled_pinvoke
{
};
// Native definition for COM marshalling of System.Enum
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2_marshaled_com
{
};

// System.Int32
struct Int32_t680FF22E76F6EFAD4375103CBBFFA0421349384C 
{
	// System.Int32 System.Int32::m_value
	int32_t ___m_value_0;
};

// System.IntPtr
struct IntPtr_t 
{
	// System.Void* System.IntPtr::m_value
	void* ___m_value_0;
};

// Facebook.Unity.Mobile.MobileFacebook
struct MobileFacebook_tFB7FCB79FF54D81C7EB30A5D40786FAB1DBFB21E  : public FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865
{
	// Facebook.Unity.ShareDialogMode Facebook.Unity.Mobile.MobileFacebook::shareDialogMode
	int32_t ___shareDialogMode_3;
};

// UnityEngine.SceneManagement.Scene
struct Scene_tA1DC762B79745EB5140F054C884855B922318356 
{
	// System.Int32 UnityEngine.SceneManagement.Scene::m_Handle
	int32_t ___m_Handle_0;
};

// System.Single
struct Single_t4530F2FF86FCB0DC29F35385CA1BD21BE294761C 
{
	// System.Single System.Single::m_value
	float ___m_value_0;
};

// System.Void
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915 
{
	union
	{
		struct
		{
		};
		uint8_t Void_t4861ACF8F4594C3437BB48B6E56783494B843915__padding[1];
	};
};

// System.Nullable`1<System.DateTime>
struct Nullable_1_tEADC262F7F8B8BC4CC0A003DBDD3CA7C1B63F9AC 
{
	// System.Boolean System.Nullable`1::hasValue
	bool ___hasValue_0;
	// T System.Nullable`1::value
	DateTime_t66193957C73913903DDAD89FEDC46139BCA5802D ___value_1;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook
struct AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD  : public MobileFacebook_tFB7FCB79FF54D81C7EB30A5D40786FAB1DBFB21E
{
	// System.Boolean Facebook.Unity.Mobile.Android.AndroidFacebook::limitEventUsage
	bool ___limitEventUsage_4;
	// Facebook.Unity.Mobile.Android.IAndroidWrapper Facebook.Unity.Mobile.Android.AndroidFacebook::androidWrapper
	RuntimeObject* ___androidWrapper_5;
	// System.String Facebook.Unity.Mobile.Android.AndroidFacebook::userID
	String_t* ___userID_6;
	// System.String Facebook.Unity.Mobile.Android.AndroidFacebook::<KeyHash>k__BackingField
	String_t* ___U3CKeyHashU3Ek__BackingField_7;
};

// System.Delegate
struct Delegate_t  : public RuntimeObject
{
	// System.IntPtr System.Delegate::method_ptr
	Il2CppMethodPointer ___method_ptr_0;
	// System.IntPtr System.Delegate::invoke_impl
	intptr_t ___invoke_impl_1;
	// System.Object System.Delegate::m_target
	RuntimeObject* ___m_target_2;
	// System.IntPtr System.Delegate::method
	intptr_t ___method_3;
	// System.IntPtr System.Delegate::delegate_trampoline
	intptr_t ___delegate_trampoline_4;
	// System.IntPtr System.Delegate::extra_arg
	intptr_t ___extra_arg_5;
	// System.IntPtr System.Delegate::method_code
	intptr_t ___method_code_6;
	// System.IntPtr System.Delegate::interp_method
	intptr_t ___interp_method_7;
	// System.IntPtr System.Delegate::interp_invoke_impl
	intptr_t ___interp_invoke_impl_8;
	// System.Reflection.MethodInfo System.Delegate::method_info
	MethodInfo_t* ___method_info_9;
	// System.Reflection.MethodInfo System.Delegate::original_method_info
	MethodInfo_t* ___original_method_info_10;
	// System.DelegateData System.Delegate::data
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data_11;
	// System.Boolean System.Delegate::method_is_virtual
	bool ___method_is_virtual_12;
};
// Native definition for P/Invoke marshalling of System.Delegate
struct Delegate_t_marshaled_pinvoke
{
	intptr_t ___method_ptr_0;
	intptr_t ___invoke_impl_1;
	Il2CppIUnknown* ___m_target_2;
	intptr_t ___method_3;
	intptr_t ___delegate_trampoline_4;
	intptr_t ___extra_arg_5;
	intptr_t ___method_code_6;
	intptr_t ___interp_method_7;
	intptr_t ___interp_invoke_impl_8;
	MethodInfo_t* ___method_info_9;
	MethodInfo_t* ___original_method_info_10;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data_11;
	int32_t ___method_is_virtual_12;
};
// Native definition for COM marshalling of System.Delegate
struct Delegate_t_marshaled_com
{
	intptr_t ___method_ptr_0;
	intptr_t ___invoke_impl_1;
	Il2CppIUnknown* ___m_target_2;
	intptr_t ___method_3;
	intptr_t ___delegate_trampoline_4;
	intptr_t ___extra_arg_5;
	intptr_t ___method_code_6;
	intptr_t ___interp_method_7;
	intptr_t ___interp_invoke_impl_8;
	MethodInfo_t* ___method_info_9;
	MethodInfo_t* ___original_method_info_10;
	DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E* ___data_11;
	int32_t ___method_is_virtual_12;
};

// System.Exception
struct Exception_t  : public RuntimeObject
{
	// System.String System.Exception::_className
	String_t* ____className_1;
	// System.String System.Exception::_message
	String_t* ____message_2;
	// System.Collections.IDictionary System.Exception::_data
	RuntimeObject* ____data_3;
	// System.Exception System.Exception::_innerException
	Exception_t* ____innerException_4;
	// System.String System.Exception::_helpURL
	String_t* ____helpURL_5;
	// System.Object System.Exception::_stackTrace
	RuntimeObject* ____stackTrace_6;
	// System.String System.Exception::_stackTraceString
	String_t* ____stackTraceString_7;
	// System.String System.Exception::_remoteStackTraceString
	String_t* ____remoteStackTraceString_8;
	// System.Int32 System.Exception::_remoteStackIndex
	int32_t ____remoteStackIndex_9;
	// System.Object System.Exception::_dynamicMethods
	RuntimeObject* ____dynamicMethods_10;
	// System.Int32 System.Exception::_HResult
	int32_t ____HResult_11;
	// System.String System.Exception::_source
	String_t* ____source_12;
	// System.Runtime.Serialization.SafeSerializationManager System.Exception::_safeSerializationManager
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager_13;
	// System.Diagnostics.StackTrace[] System.Exception::captured_traces
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces_14;
	// System.IntPtr[] System.Exception::native_trace_ips
	IntPtrU5BU5D_tFD177F8C806A6921AD7150264CCC62FA00CAD832* ___native_trace_ips_15;
	// System.Int32 System.Exception::caught_in_unmanaged
	int32_t ___caught_in_unmanaged_16;
};
// Native definition for P/Invoke marshalling of System.Exception
struct Exception_t_marshaled_pinvoke
{
	char* ____className_1;
	char* ____message_2;
	RuntimeObject* ____data_3;
	Exception_t_marshaled_pinvoke* ____innerException_4;
	char* ____helpURL_5;
	Il2CppIUnknown* ____stackTrace_6;
	char* ____stackTraceString_7;
	char* ____remoteStackTraceString_8;
	int32_t ____remoteStackIndex_9;
	Il2CppIUnknown* ____dynamicMethods_10;
	int32_t ____HResult_11;
	char* ____source_12;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager_13;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces_14;
	Il2CppSafeArray/*NONE*/* ___native_trace_ips_15;
	int32_t ___caught_in_unmanaged_16;
};
// Native definition for COM marshalling of System.Exception
struct Exception_t_marshaled_com
{
	Il2CppChar* ____className_1;
	Il2CppChar* ____message_2;
	RuntimeObject* ____data_3;
	Exception_t_marshaled_com* ____innerException_4;
	Il2CppChar* ____helpURL_5;
	Il2CppIUnknown* ____stackTrace_6;
	Il2CppChar* ____stackTraceString_7;
	Il2CppChar* ____remoteStackTraceString_8;
	int32_t ____remoteStackIndex_9;
	Il2CppIUnknown* ____dynamicMethods_10;
	int32_t ____HResult_11;
	Il2CppChar* ____source_12;
	SafeSerializationManager_tCBB85B95DFD1634237140CD892E82D06ECB3F5E6* ____safeSerializationManager_13;
	StackTraceU5BU5D_t32FBCB20930EAF5BAE3F450FF75228E5450DA0DF* ___captured_traces_14;
	Il2CppSafeArray/*NONE*/* ___native_trace_ips_15;
	int32_t ___caught_in_unmanaged_16;
};

// Facebook.Unity.Mobile.IOS.IOSFacebook
struct IOSFacebook_tC5DD1DAB3274516F7194A166C4C9D5C2900A99F4  : public MobileFacebook_tFB7FCB79FF54D81C7EB30A5D40786FAB1DBFB21E
{
	// System.Boolean Facebook.Unity.Mobile.IOS.IOSFacebook::limitEventUsage
	bool ___limitEventUsage_4;
	// Facebook.Unity.Mobile.IOS.IIOSWrapper Facebook.Unity.Mobile.IOS.IOSFacebook::iosWrapper
	RuntimeObject* ___iosWrapper_5;
	// System.String Facebook.Unity.Mobile.IOS.IOSFacebook::userID
	String_t* ___userID_6;
};

// UnityEngine.Object
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C  : public RuntimeObject
{
	// System.IntPtr UnityEngine.Object::m_CachedPtr
	intptr_t ___m_CachedPtr_0;
};
// Native definition for P/Invoke marshalling of UnityEngine.Object
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke
{
	intptr_t ___m_CachedPtr_0;
};
// Native definition for COM marshalling of UnityEngine.Object
struct Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com
{
	intptr_t ___m_CachedPtr_0;
};

// Facebook.Unity.ResultBase
struct ResultBase_tCBD64014EA4B0EDCCAFD59C03894FD853F79B1A1  : public RuntimeObject
{
	// System.String Facebook.Unity.ResultBase::<Error>k__BackingField
	String_t* ___U3CErrorU3Ek__BackingField_0;
	// System.Collections.Generic.IDictionary`2<System.String,System.String> Facebook.Unity.ResultBase::<ErrorDictionary>k__BackingField
	RuntimeObject* ___U3CErrorDictionaryU3Ek__BackingField_1;
	// System.Collections.Generic.IDictionary`2<System.String,System.Object> Facebook.Unity.ResultBase::<ResultDictionary>k__BackingField
	RuntimeObject* ___U3CResultDictionaryU3Ek__BackingField_2;
	// System.String Facebook.Unity.ResultBase::<RawResult>k__BackingField
	String_t* ___U3CRawResultU3Ek__BackingField_3;
	// System.Boolean Facebook.Unity.ResultBase::<Cancelled>k__BackingField
	bool ___U3CCancelledU3Ek__BackingField_4;
	// System.String Facebook.Unity.ResultBase::<CallbackId>k__BackingField
	String_t* ___U3CCallbackIdU3Ek__BackingField_5;
	// System.Nullable`1<System.Int64> Facebook.Unity.ResultBase::<CanvasErrorCode>k__BackingField
	Nullable_1_t365991B3904FDA7642A788423B28692FDC7CDB17 ___U3CCanvasErrorCodeU3Ek__BackingField_6;
};

// System.RuntimeTypeHandle
struct RuntimeTypeHandle_t332A452B8B6179E4469B69525D0FE82A88030F7B 
{
	// System.IntPtr System.RuntimeTypeHandle::value
	intptr_t ___value_0;
};

// Facebook.Unity.AccessToken
struct AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346  : public RuntimeObject
{
	// System.String Facebook.Unity.AccessToken::<TokenString>k__BackingField
	String_t* ___U3CTokenStringU3Ek__BackingField_1;
	// System.DateTime Facebook.Unity.AccessToken::<ExpirationTime>k__BackingField
	DateTime_t66193957C73913903DDAD89FEDC46139BCA5802D ___U3CExpirationTimeU3Ek__BackingField_2;
	// System.Collections.Generic.IEnumerable`1<System.String> Facebook.Unity.AccessToken::<Permissions>k__BackingField
	RuntimeObject* ___U3CPermissionsU3Ek__BackingField_3;
	// System.String Facebook.Unity.AccessToken::<UserId>k__BackingField
	String_t* ___U3CUserIdU3Ek__BackingField_4;
	// System.Nullable`1<System.DateTime> Facebook.Unity.AccessToken::<LastRefresh>k__BackingField
	Nullable_1_tEADC262F7F8B8BC4CC0A003DBDD3CA7C1B63F9AC ___U3CLastRefreshU3Ek__BackingField_5;
	// System.String Facebook.Unity.AccessToken::<GraphDomain>k__BackingField
	String_t* ___U3CGraphDomainU3Ek__BackingField_6;
};

// Facebook.Unity.AppLinkResult
struct AppLinkResult_t7B3A5BB1C71DF6D48BBC7B508AE365E8D4A79143  : public ResultBase_tCBD64014EA4B0EDCCAFD59C03894FD853F79B1A1
{
	// System.String Facebook.Unity.AppLinkResult::<Url>k__BackingField
	String_t* ___U3CUrlU3Ek__BackingField_7;
	// System.String Facebook.Unity.AppLinkResult::<TargetUrl>k__BackingField
	String_t* ___U3CTargetUrlU3Ek__BackingField_8;
	// System.String Facebook.Unity.AppLinkResult::<Ref>k__BackingField
	String_t* ___U3CRefU3Ek__BackingField_9;
	// System.Collections.Generic.IDictionary`2<System.String,System.Object> Facebook.Unity.AppLinkResult::<Extras>k__BackingField
	RuntimeObject* ___U3CExtrasU3Ek__BackingField_10;
};

// Facebook.Unity.AppRequestResult
struct AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2  : public ResultBase_tCBD64014EA4B0EDCCAFD59C03894FD853F79B1A1
{
	// System.String Facebook.Unity.AppRequestResult::<RequestID>k__BackingField
	String_t* ___U3CRequestIDU3Ek__BackingField_7;
	// System.Collections.Generic.IEnumerable`1<System.String> Facebook.Unity.AppRequestResult::<To>k__BackingField
	RuntimeObject* ___U3CToU3Ek__BackingField_8;
};

// UnityEngine.Component
struct Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3  : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C
{
};

// UnityEngine.GameObject
struct GameObject_t76FEDD663AB33C991A9C9A23129337651094216F  : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C
{
};

// Facebook.Unity.LoginResult
struct LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1  : public ResultBase_tCBD64014EA4B0EDCCAFD59C03894FD853F79B1A1
{
	// Facebook.Unity.AccessToken Facebook.Unity.LoginResult::<AccessToken>k__BackingField
	AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346* ___U3CAccessTokenU3Ek__BackingField_14;
	// Facebook.Unity.AuthenticationToken Facebook.Unity.LoginResult::<AuthenticationToken>k__BackingField
	AuthenticationToken_t925F4C42BE7D08897D579F0A55D5682704237B8C* ___U3CAuthenticationTokenU3Ek__BackingField_15;
};

// System.MulticastDelegate
struct MulticastDelegate_t  : public Delegate_t
{
	// System.Delegate[] System.MulticastDelegate::delegates
	DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771* ___delegates_13;
};
// Native definition for P/Invoke marshalling of System.MulticastDelegate
struct MulticastDelegate_t_marshaled_pinvoke : public Delegate_t_marshaled_pinvoke
{
	Delegate_t_marshaled_pinvoke** ___delegates_13;
};
// Native definition for COM marshalling of System.MulticastDelegate
struct MulticastDelegate_t_marshaled_com : public Delegate_t_marshaled_com
{
	Delegate_t_marshaled_com** ___delegates_13;
};

// Facebook.Unity.PayResult
struct PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08  : public ResultBase_tCBD64014EA4B0EDCCAFD59C03894FD853F79B1A1
{
};

// Facebook.Unity.Profile
struct Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7  : public RuntimeObject
{
	// System.String Facebook.Unity.Profile::<UserID>k__BackingField
	String_t* ___U3CUserIDU3Ek__BackingField_0;
	// System.String Facebook.Unity.Profile::<FirstName>k__BackingField
	String_t* ___U3CFirstNameU3Ek__BackingField_1;
	// System.String Facebook.Unity.Profile::<MiddleName>k__BackingField
	String_t* ___U3CMiddleNameU3Ek__BackingField_2;
	// System.String Facebook.Unity.Profile::<LastName>k__BackingField
	String_t* ___U3CLastNameU3Ek__BackingField_3;
	// System.String Facebook.Unity.Profile::<Name>k__BackingField
	String_t* ___U3CNameU3Ek__BackingField_4;
	// System.String Facebook.Unity.Profile::<Email>k__BackingField
	String_t* ___U3CEmailU3Ek__BackingField_5;
	// System.String Facebook.Unity.Profile::<ImageURL>k__BackingField
	String_t* ___U3CImageURLU3Ek__BackingField_6;
	// System.String Facebook.Unity.Profile::<LinkURL>k__BackingField
	String_t* ___U3CLinkURLU3Ek__BackingField_7;
	// System.String[] Facebook.Unity.Profile::<FriendIDs>k__BackingField
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___U3CFriendIDsU3Ek__BackingField_8;
	// System.Nullable`1<System.DateTime> Facebook.Unity.Profile::<Birthday>k__BackingField
	Nullable_1_tEADC262F7F8B8BC4CC0A003DBDD3CA7C1B63F9AC ___U3CBirthdayU3Ek__BackingField_9;
	// Facebook.Unity.UserAgeRange Facebook.Unity.Profile::<AgeRange>k__BackingField
	UserAgeRange_t3074872BAC5CB213D6E7245C5C9407CA12B7321A* ___U3CAgeRangeU3Ek__BackingField_10;
	// Facebook.Unity.FBLocation Facebook.Unity.Profile::<Hometown>k__BackingField
	FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* ___U3CHometownU3Ek__BackingField_11;
	// Facebook.Unity.FBLocation Facebook.Unity.Profile::<Location>k__BackingField
	FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* ___U3CLocationU3Ek__BackingField_12;
	// System.String Facebook.Unity.Profile::<Gender>k__BackingField
	String_t* ___U3CGenderU3Ek__BackingField_13;
};

// UnityEngine.ScriptableObject
struct ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A  : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C
{
};
// Native definition for P/Invoke marshalling of UnityEngine.ScriptableObject
struct ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A_marshaled_pinvoke : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_pinvoke
{
};
// Native definition for COM marshalling of UnityEngine.ScriptableObject
struct ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A_marshaled_com : public Object_tC12DECB6760A7F2CBF65D9DCF18D044C2D97152C_marshaled_com
{
};

// Facebook.Unity.ShareResult
struct ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0  : public ResultBase_tCBD64014EA4B0EDCCAFD59C03894FD853F79B1A1
{
	// System.String Facebook.Unity.ShareResult::<PostId>k__BackingField
	String_t* ___U3CPostIdU3Ek__BackingField_7;
};

// System.SystemException
struct SystemException_tCC48D868298F4C0705279823E34B00F4FBDB7295  : public Exception_t
{
};

// System.Type
struct Type_t  : public MemberInfo_t
{
	// System.RuntimeTypeHandle System.Type::_impl
	RuntimeTypeHandle_t332A452B8B6179E4469B69525D0FE82A88030F7B ____impl_8;
};

// Facebook.Unity.Utilities/Callback`1<System.Object>
struct Callback_1_t244FCFB1C42CF22CE5AF6954370376FF384F449A  : public MulticastDelegate_t
{
};

// Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer>
struct Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAccessTokenRefreshResult>
struct FacebookDelegate_1_t0A787A64D3187E98A865A198DDF444C008C79F35  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppLinkResult>
struct FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppRequestResult>
struct FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ICatalogResult>
struct FacebookDelegate_1_tA8F51BBA2E7364881368E0CBED892F561162C32E  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IConsumePurchaseResult>
struct FacebookDelegate_1_tF96C838D4977CEFDB874E77F7A56B1CFECF4D8DD  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGamingServicesFriendFinderResult>
struct FacebookDelegate_1_t55619BCDC758CB1C82277E6D8BA44ACA47C03DE2  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGraphResult>
struct FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IIAPReadyResult>
struct FacebookDelegate_1_tA2A619ACCF137D8094955A556E78A66749C43F74  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInitCloudGameResult>
struct FacebookDelegate_1_tB829CDA8266CAB15D1703F5904BB2F8592CA60E8  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInterstitialAdResult>
struct FacebookDelegate_1_t6BAE034F2CC3270BFC282A9D0DEB58CC5F91C265  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ILoginResult>
struct FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IMediaUploadResult>
struct FacebookDelegate_1_t7CA00F6A27B3FE85139590EFEA03B7C7C0D4A66D  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IOpenAppStoreResult>
struct FacebookDelegate_1_t5E457BF6B03D4AD72D97F1848A0612B675747CFD  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayResult>
struct FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayloadResult>
struct FacebookDelegate_1_tB6B04FA63685B9A0D41AB08E3E5BC96A72D7A1D7  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchaseResult>
struct FacebookDelegate_1_tBD274D4AED39A6A24A22981D38D3CBF447CF036E  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchasesResult>
struct FacebookDelegate_1_t61D056F5D405CB3DF2F643449D9C03A64AB8E818  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IRewardedVideoResult>
struct FacebookDelegate_1_t9C5748E8AC36242F3F03171AA1E6306E60860ACD  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IScheduleAppToUserNotificationResult>
struct FacebookDelegate_1_t623E520D5B99523D410AEE72AB9FF4901DC356AC  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ISessionScoreResult>
struct FacebookDelegate_1_t610D359C8E47669CC0D9F0B6CA9DF9240BF80267  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IShareResult>
struct FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentResult>
struct FacebookDelegate_1_tD9C9BBFE6AD0931C935E391D858C37BEBF14BBA3  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentScoreResult>
struct FacebookDelegate_1_t474682078C474498C8D4805F13E9077763B39255  : public MulticastDelegate_t
{
};

// Facebook.Unity.FacebookDelegate`1<System.Object>
struct FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9  : public MulticastDelegate_t
{
};

// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.Object>
struct Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF  : public MulticastDelegate_t
{
};

// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.String>
struct Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088  : public MulticastDelegate_t
{
};

// UnityEngine.Events.UnityAction`2<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>
struct UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A  : public MulticastDelegate_t
{
};

// UnityEngine.Behaviour
struct Behaviour_t01970CFBBA658497AE30F311C447DB0440BAB7FA  : public Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3
{
};

// Facebook.Unity.FB
struct FB_tD6AF917A642BEC6920761C8E4AD4013414829013  : public ScriptableObject_tB3BFDB921A1B1795B38A5417D3B97A89A140436A
{
};

// Facebook.Unity.HideUnityDelegate
struct HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE  : public MulticastDelegate_t
{
};

// Facebook.Unity.InitDelegate
struct InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F  : public MulticastDelegate_t
{
};

// Facebook.Unity.LoginStatusResult
struct LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7  : public LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1
{
	// System.Boolean Facebook.Unity.LoginStatusResult::<Failed>k__BackingField
	bool ___U3CFailedU3Ek__BackingField_17;
};

// System.NotImplementedException
struct NotImplementedException_t6366FE4DCF15094C51F4833B91A2AE68D4DA90E8  : public SystemException_tCC48D868298F4C0705279823E34B00F4FBDB7295
{
};

// UnityEngine.Transform
struct Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1  : public Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3
{
};

// UnityEngine.MonoBehaviour
struct MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71  : public Behaviour_t01970CFBBA658497AE30F311C447DB0440BAB7FA
{
};

// Facebook.Unity.FacebookGameObject
struct FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B  : public MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71
{
	// Facebook.Unity.IFacebookImplementation Facebook.Unity.FacebookGameObject::<Facebook>k__BackingField
	RuntimeObject* ___U3CFacebookU3Ek__BackingField_4;
};

// Facebook.Unity.Canvas.JsBridge
struct JsBridge_t338341DF8272C935803F827012749337260971DA  : public MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71
{
	// Facebook.Unity.Canvas.ICanvasFacebookCallbackHandler Facebook.Unity.Canvas.JsBridge::facebook
	RuntimeObject* ___facebook_4;
};

// Facebook.Unity.FB/CompiledFacebookLoader
struct CompiledFacebookLoader_tDDF38F9F03C32FC27FB7546387BA7B9DBDCF5089  : public MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71
{
};

// Facebook.Unity.Mobile.Android.AndroidFacebookLoader
struct AndroidFacebookLoader_t581FB6217BD8B55FE1012FCF1AC8EDABE25320E9  : public CompiledFacebookLoader_tDDF38F9F03C32FC27FB7546387BA7B9DBDCF5089
{
};

// Facebook.Unity.Canvas.CanvasFacebookGameObject
struct CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46  : public FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B
{
};

// Facebook.Unity.Canvas.CanvasFacebookLoader
struct CanvasFacebookLoader_t50D8266C527416BEBCBA1355E4513AD3DEBC71EA  : public CompiledFacebookLoader_tDDF38F9F03C32FC27FB7546387BA7B9DBDCF5089
{
};

// Facebook.Unity.Mobile.IOS.IOSFacebookLoader
struct IOSFacebookLoader_tC4449925F5E12605B6DAC60A202C0EC3ECB7B242  : public CompiledFacebookLoader_tDDF38F9F03C32FC27FB7546387BA7B9DBDCF5089
{
};

// Facebook.Unity.Mobile.MobileFacebookGameObject
struct MobileFacebookGameObject_t105BCEF7BF0751B141C332580EE375559D10BB7C  : public FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B
{
};

// Facebook.Unity.Mobile.Android.AndroidFacebookGameObject
struct AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378  : public MobileFacebookGameObject_t105BCEF7BF0751B141C332580EE375559D10BB7C
{
};

// Facebook.Unity.Mobile.IOS.IOSFacebookGameObject
struct IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA  : public MobileFacebookGameObject_t105BCEF7BF0751B141C332580EE375559D10BB7C
{
};

// System.Collections.Generic.Dictionary`2<System.String,System.Object>

// System.Collections.Generic.Dictionary`2<System.String,System.Object>

// System.Collections.Generic.Dictionary`2<System.String,System.String>

// System.Collections.Generic.Dictionary`2<System.String,System.String>

// System.Collections.Generic.List`1<System.String>
struct List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD_StaticFields
{
	// T[] System.Collections.Generic.List`1::s_emptyArray
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___s_emptyArray_5;
};

// System.Collections.Generic.List`1<System.String>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IAppLinkResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IAppLinkResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IAppRequestResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IAppRequestResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ICatalogResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ICatalogResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IConsumePurchaseResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IConsumePurchaseResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IIAPReadyResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IIAPReadyResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IInitCloudGameResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IInitCloudGameResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IInterstitialAdResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IInterstitialAdResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ILoginResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ILoginResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IMediaUploadResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IMediaUploadResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IOpenAppStoreResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IOpenAppStoreResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPayResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPayResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPayloadResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPayloadResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchaseResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchaseResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchasesResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchasesResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IRewardedVideoResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IRewardedVideoResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ISessionScoreResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ISessionScoreResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IShareResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.IShareResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentScoreResult>

// Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentScoreResult>

// Facebook.Unity.MethodCall`1<System.Object>

// Facebook.Unity.MethodCall`1<System.Object>

// System.Reflection.Assembly

// System.Reflection.Assembly

// Facebook.Unity.AuthenticationToken

// Facebook.Unity.AuthenticationToken

// Facebook.Unity.CallbackManager

// Facebook.Unity.CallbackManager

// System.Globalization.CultureInfo
struct CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_StaticFields
{
	// System.Globalization.CultureInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::invariant_culture_info
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___invariant_culture_info_0;
	// System.Object System.Globalization.CultureInfo::shared_table_lock
	RuntimeObject* ___shared_table_lock_1;
	// System.Globalization.CultureInfo System.Globalization.CultureInfo::default_current_culture
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___default_current_culture_2;
	// System.Globalization.CultureInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::s_DefaultThreadCurrentUICulture
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___s_DefaultThreadCurrentUICulture_34;
	// System.Globalization.CultureInfo modreq(System.Runtime.CompilerServices.IsVolatile) System.Globalization.CultureInfo::s_DefaultThreadCurrentCulture
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___s_DefaultThreadCurrentCulture_35;
	// System.Collections.Generic.Dictionary`2<System.Int32,System.Globalization.CultureInfo> System.Globalization.CultureInfo::shared_by_number
	Dictionary_2_t9FA6D82CAFC18769F7515BB51D1C56DAE09381C3* ___shared_by_number_36;
	// System.Collections.Generic.Dictionary`2<System.String,System.Globalization.CultureInfo> System.Globalization.CultureInfo::shared_by_name
	Dictionary_2_tE1603CE612C16451D1E56FF4D4859D4FE4087C28* ___shared_by_name_37;
	// System.Globalization.CultureInfo System.Globalization.CultureInfo::s_UserPreferredCultureInfoInAppX
	CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* ___s_UserPreferredCultureInfoInAppX_38;
	// System.Boolean System.Globalization.CultureInfo::IsTaiwanSku
	bool ___IsTaiwanSku_39;
};

// System.Globalization.CultureInfo

// Facebook.Unity.FBLocation

// Facebook.Unity.FBLocation

// Facebook.Unity.FacebookBase

// Facebook.Unity.FacebookBase

// Facebook.Unity.MethodArguments

// Facebook.Unity.MethodArguments

// Facebook.Unity.ResultContainer

// Facebook.Unity.ResultContainer

// System.String
struct String_t_StaticFields
{
	// System.String System.String::Empty
	String_t* ___Empty_6;
};

// System.String

// System.Uri
struct Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E_StaticFields
{
	// System.String System.Uri::UriSchemeFile
	String_t* ___UriSchemeFile_0;
	// System.String System.Uri::UriSchemeFtp
	String_t* ___UriSchemeFtp_1;
	// System.String System.Uri::UriSchemeGopher
	String_t* ___UriSchemeGopher_2;
	// System.String System.Uri::UriSchemeHttp
	String_t* ___UriSchemeHttp_3;
	// System.String System.Uri::UriSchemeHttps
	String_t* ___UriSchemeHttps_4;
	// System.String System.Uri::UriSchemeWs
	String_t* ___UriSchemeWs_5;
	// System.String System.Uri::UriSchemeWss
	String_t* ___UriSchemeWss_6;
	// System.String System.Uri::UriSchemeMailto
	String_t* ___UriSchemeMailto_7;
	// System.String System.Uri::UriSchemeNews
	String_t* ___UriSchemeNews_8;
	// System.String System.Uri::UriSchemeNntp
	String_t* ___UriSchemeNntp_9;
	// System.String System.Uri::UriSchemeNetTcp
	String_t* ___UriSchemeNetTcp_10;
	// System.String System.Uri::UriSchemeNetPipe
	String_t* ___UriSchemeNetPipe_11;
	// System.String System.Uri::SchemeDelimiter
	String_t* ___SchemeDelimiter_12;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.Uri::s_ConfigInitialized
	bool ___s_ConfigInitialized_23;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.Uri::s_ConfigInitializing
	bool ___s_ConfigInitializing_24;
	// System.UriIdnScope modreq(System.Runtime.CompilerServices.IsVolatile) System.Uri::s_IdnScope
	int32_t ___s_IdnScope_25;
	// System.Boolean modreq(System.Runtime.CompilerServices.IsVolatile) System.Uri::s_IriParsing
	bool ___s_IriParsing_26;
	// System.Boolean System.Uri::useDotNetRelativeOrAbsolute
	bool ___useDotNetRelativeOrAbsolute_27;
	// System.Boolean System.Uri::IsWindowsFileSystem
	bool ___IsWindowsFileSystem_29;
	// System.Object System.Uri::s_initLock
	RuntimeObject* ___s_initLock_30;
	// System.Char[] System.Uri::HexLowerChars
	CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB* ___HexLowerChars_34;
	// System.Char[] System.Uri::_WSchars
	CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB* ____WSchars_35;
};

// System.Uri

// Facebook.Unity.UserAgeRange

// Facebook.Unity.UserAgeRange

// Facebook.Unity.Mobile.Android.AndroidFacebook/<>c
struct U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields
{
	// Facebook.Unity.Mobile.Android.AndroidFacebook/<>c Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::<>9
	U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* ___U3CU3E9_0;
	// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.String> Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::<>9__65_0
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* ___U3CU3E9__65_0_1;
	// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.Object> Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::<>9__65_1
	Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* ___U3CU3E9__65_1_2;
	// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.String> Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::<>9__66_0
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* ___U3CU3E9__66_0_3;
	// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.Object> Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::<>9__66_1
	Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* ___U3CU3E9__66_1_4;
};

// Facebook.Unity.Mobile.Android.AndroidFacebook/<>c

// Facebook.Unity.Canvas.CanvasFacebook/<>c
struct U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_StaticFields
{
	// Facebook.Unity.Canvas.CanvasFacebook/<>c Facebook.Unity.Canvas.CanvasFacebook/<>c::<>9
	U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8* ___U3CU3E9_0;
	// Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer> Facebook.Unity.Canvas.CanvasFacebook/<>c::<>9__40_0
	Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* ___U3CU3E9__40_0_1;
};

// Facebook.Unity.Canvas.CanvasFacebook/<>c

// Facebook.Unity.Canvas.CanvasFacebook/<>c__DisplayClass47_0

// Facebook.Unity.Canvas.CanvasFacebook/<>c__DisplayClass47_0

// Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict

// Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict

// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IAppRequestResult>

// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IAppRequestResult>

// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IPayResult>

// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IPayResult>

// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IShareResult>

// Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IShareResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppLinkResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppLinkResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppRequestResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppRequestResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ICatalogResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ICatalogResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IConsumePurchaseResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IConsumePurchaseResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IIAPReadyResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IIAPReadyResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInitCloudGameResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInitCloudGameResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInterstitialAdResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInterstitialAdResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ILoginResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ILoginResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IMediaUploadResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IMediaUploadResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IOpenAppStoreResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IOpenAppStoreResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPayloadResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPayloadResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchaseResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchaseResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchasesResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchasesResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IRewardedVideoResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IRewardedVideoResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ISessionScoreResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ISessionScoreResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IShareResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IShareResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentScoreResult>

// Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentScoreResult>

// System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>

// System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>

// System.Collections.Generic.KeyValuePair`2<System.String,System.Object>

// System.Collections.Generic.KeyValuePair`2<System.String,System.Object>

// System.Collections.Generic.KeyValuePair`2<System.String,System.String>

// System.Collections.Generic.KeyValuePair`2<System.String,System.String>

// System.Nullable`1<System.Int32>

// System.Nullable`1<System.Int32>

// System.Nullable`1<System.Int32Enum>

// System.Nullable`1<System.Int32Enum>

// System.Nullable`1<Facebook.Unity.OGActionType>

// System.Nullable`1<Facebook.Unity.OGActionType>

// System.Nullable`1<System.Single>

// System.Nullable`1<System.Single>

// System.Boolean
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	// System.String System.Boolean::TrueString
	String_t* ___TrueString_5;
	// System.String System.Boolean::FalseString
	String_t* ___FalseString_6;
};

// System.Boolean

// Facebook.Unity.Canvas.CanvasFacebook

// Facebook.Unity.Canvas.CanvasFacebook

// System.Char
struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17_StaticFields
{
	// System.Byte[] System.Char::s_categoryForLatin1
	ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* ___s_categoryForLatin1_3;
};

// System.Char

// System.Enum
struct Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2_StaticFields
{
	// System.Char[] System.Enum::enumSeperatorCharArray
	CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB* ___enumSeperatorCharArray_0;
};

// System.Enum

// System.Int32

// System.Int32

// System.IntPtr
struct IntPtr_t_StaticFields
{
	// System.IntPtr System.IntPtr::Zero
	intptr_t ___Zero_1;
};

// System.IntPtr

// Facebook.Unity.Mobile.MobileFacebook

// Facebook.Unity.Mobile.MobileFacebook

// UnityEngine.SceneManagement.Scene

// UnityEngine.SceneManagement.Scene

// System.Single

// System.Single

// System.Void

// System.Void

// Facebook.Unity.Mobile.Android.AndroidFacebook

// Facebook.Unity.Mobile.Android.AndroidFacebook

// System.Exception
struct Exception_t_StaticFields
{
	// System.Object System.Exception::s_EDILock
	RuntimeObject* ___s_EDILock_0;
};

// System.Exception

// Facebook.Unity.Mobile.IOS.IOSFacebook

// Facebook.Unity.Mobile.IOS.IOSFacebook

// Facebook.Unity.AccessToken
struct AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346_StaticFields
{
	// Facebook.Unity.AccessToken Facebook.Unity.AccessToken::<CurrentAccessToken>k__BackingField
	AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346* ___U3CCurrentAccessTokenU3Ek__BackingField_0;
};

// Facebook.Unity.AccessToken

// Facebook.Unity.AppLinkResult

// Facebook.Unity.AppLinkResult

// Facebook.Unity.AppRequestResult

// Facebook.Unity.AppRequestResult

// UnityEngine.Component

// UnityEngine.Component

// UnityEngine.GameObject

// UnityEngine.GameObject

// Facebook.Unity.LoginResult
struct LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_StaticFields
{
	// System.String Facebook.Unity.LoginResult::UserIdKey
	String_t* ___UserIdKey_7;
	// System.String Facebook.Unity.LoginResult::ExpirationTimestampKey
	String_t* ___ExpirationTimestampKey_8;
	// System.String Facebook.Unity.LoginResult::PermissionsKey
	String_t* ___PermissionsKey_9;
	// System.String Facebook.Unity.LoginResult::AccessTokenKey
	String_t* ___AccessTokenKey_10;
	// System.String Facebook.Unity.LoginResult::GraphDomain
	String_t* ___GraphDomain_11;
	// System.String Facebook.Unity.LoginResult::AuthTokenString
	String_t* ___AuthTokenString_12;
	// System.String Facebook.Unity.LoginResult::AuthNonce
	String_t* ___AuthNonce_13;
};

// Facebook.Unity.LoginResult

// Facebook.Unity.PayResult

// Facebook.Unity.PayResult

// Facebook.Unity.Profile

// Facebook.Unity.Profile

// Facebook.Unity.ShareResult

// Facebook.Unity.ShareResult

// System.Type
struct Type_t_StaticFields
{
	// System.Reflection.Binder modreq(System.Runtime.CompilerServices.IsVolatile) System.Type::s_defaultBinder
	Binder_t91BFCE95A7057FADF4D8A1A342AFE52872246235* ___s_defaultBinder_0;
	// System.Char System.Type::Delimiter
	Il2CppChar ___Delimiter_1;
	// System.Type[] System.Type::EmptyTypes
	TypeU5BU5D_t97234E1129B564EB38B8D85CAC2AD8B5B9522FFB* ___EmptyTypes_2;
	// System.Object System.Type::Missing
	RuntimeObject* ___Missing_3;
	// System.Reflection.MemberFilter System.Type::FilterAttribute
	MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553* ___FilterAttribute_4;
	// System.Reflection.MemberFilter System.Type::FilterName
	MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553* ___FilterName_5;
	// System.Reflection.MemberFilter System.Type::FilterNameIgnoreCase
	MemberFilter_tF644F1AE82F611B677CE1964D5A3277DDA21D553* ___FilterNameIgnoreCase_6;
};

// System.Type

// Facebook.Unity.Utilities/Callback`1<System.Object>

// Facebook.Unity.Utilities/Callback`1<System.Object>

// Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer>

// Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAccessTokenRefreshResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAccessTokenRefreshResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppLinkResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppLinkResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppRequestResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppRequestResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ICatalogResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ICatalogResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IConsumePurchaseResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IConsumePurchaseResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGamingServicesFriendFinderResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGamingServicesFriendFinderResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGraphResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGraphResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IIAPReadyResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IIAPReadyResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInitCloudGameResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInitCloudGameResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInterstitialAdResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInterstitialAdResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ILoginResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ILoginResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IMediaUploadResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IMediaUploadResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IOpenAppStoreResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IOpenAppStoreResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayloadResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayloadResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchaseResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchaseResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchasesResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchasesResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IRewardedVideoResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IRewardedVideoResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IScheduleAppToUserNotificationResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IScheduleAppToUserNotificationResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ISessionScoreResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ISessionScoreResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IShareResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IShareResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentScoreResult>

// Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentScoreResult>

// Facebook.Unity.FacebookDelegate`1<System.Object>

// Facebook.Unity.FacebookDelegate`1<System.Object>

// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.Object>

// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.Object>

// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.String>

// System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.String>

// UnityEngine.Events.UnityAction`2<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>

// UnityEngine.Events.UnityAction`2<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>

// Facebook.Unity.FB
struct FB_tD6AF917A642BEC6920761C8E4AD4013414829013_StaticFields
{
	// Facebook.Unity.IFacebook Facebook.Unity.FB::facebook
	RuntimeObject* ___facebook_5;
	// System.Boolean Facebook.Unity.FB::isInitCalled
	bool ___isInitCalled_6;
	// System.String Facebook.Unity.FB::facebookDomain
	String_t* ___facebookDomain_7;
	// System.String Facebook.Unity.FB::gamingDomain
	String_t* ___gamingDomain_8;
	// System.String Facebook.Unity.FB::graphApiVersion
	String_t* ___graphApiVersion_9;
	// System.String Facebook.Unity.FB::<AppId>k__BackingField
	String_t* ___U3CAppIdU3Ek__BackingField_10;
	// System.String Facebook.Unity.FB::<ClientToken>k__BackingField
	String_t* ___U3CClientTokenU3Ek__BackingField_11;
	// Facebook.Unity.FB/OnDLLLoaded Facebook.Unity.FB::<OnDLLLoadedDelegate>k__BackingField
	OnDLLLoaded_t9F6A891D0400EBD1F1267ED3C20ABDCEBCA8DED7* ___U3COnDLLLoadedDelegateU3Ek__BackingField_12;
};

// Facebook.Unity.FB

// Facebook.Unity.HideUnityDelegate

// Facebook.Unity.HideUnityDelegate

// Facebook.Unity.InitDelegate

// Facebook.Unity.InitDelegate

// Facebook.Unity.LoginStatusResult
struct LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7_StaticFields
{
	// System.String Facebook.Unity.LoginStatusResult::FailedKey
	String_t* ___FailedKey_16;
};

// Facebook.Unity.LoginStatusResult

// System.NotImplementedException

// System.NotImplementedException

// UnityEngine.Transform

// UnityEngine.Transform

// UnityEngine.MonoBehaviour

// UnityEngine.MonoBehaviour

// Facebook.Unity.FacebookGameObject

// Facebook.Unity.FacebookGameObject

// Facebook.Unity.Canvas.JsBridge

// Facebook.Unity.Canvas.JsBridge

// Facebook.Unity.FB/CompiledFacebookLoader

// Facebook.Unity.FB/CompiledFacebookLoader

// Facebook.Unity.Mobile.Android.AndroidFacebookLoader

// Facebook.Unity.Mobile.Android.AndroidFacebookLoader

// Facebook.Unity.Canvas.CanvasFacebookGameObject

// Facebook.Unity.Canvas.CanvasFacebookGameObject

// Facebook.Unity.Canvas.CanvasFacebookLoader

// Facebook.Unity.Canvas.CanvasFacebookLoader

// Facebook.Unity.Mobile.IOS.IOSFacebookLoader

// Facebook.Unity.Mobile.IOS.IOSFacebookLoader

// Facebook.Unity.Mobile.MobileFacebookGameObject

// Facebook.Unity.Mobile.MobileFacebookGameObject

// Facebook.Unity.Mobile.Android.AndroidFacebookGameObject

// Facebook.Unity.Mobile.Android.AndroidFacebookGameObject

// Facebook.Unity.Mobile.IOS.IOSFacebookGameObject

// Facebook.Unity.Mobile.IOS.IOSFacebookGameObject
#ifdef __clang__
#pragma clang diagnostic pop
#endif
// System.String[]
struct StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248  : public RuntimeArray
{
	ALIGN_FIELD (8) String_t* m_Items[1];

	inline String_t* GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline String_t** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, String_t* value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
	inline String_t* GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline String_t** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, String_t* value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
};
// System.Object[]
struct ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918  : public RuntimeArray
{
	ALIGN_FIELD (8) RuntimeObject* m_Items[1];

	inline RuntimeObject* GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline RuntimeObject** GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, RuntimeObject* value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
	inline RuntimeObject* GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline RuntimeObject** GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, RuntimeObject* value)
	{
		m_Items[index] = value;
		Il2CppCodeGenWriteBarrier((void**)m_Items + index, (void*)value);
	}
};
// System.Char[]
struct CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB  : public RuntimeArray
{
	ALIGN_FIELD (8) Il2CppChar m_Items[1];

	inline Il2CppChar GetAt(il2cpp_array_size_t index) const
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items[index];
	}
	inline Il2CppChar* GetAddressAt(il2cpp_array_size_t index)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		return m_Items + index;
	}
	inline void SetAt(il2cpp_array_size_t index, Il2CppChar value)
	{
		IL2CPP_ARRAY_BOUNDS_CHECK(index, (uint32_t)(this)->max_length);
		m_Items[index] = value;
	}
	inline Il2CppChar GetAtUnchecked(il2cpp_array_size_t index) const
	{
		return m_Items[index];
	}
	inline Il2CppChar* GetAddressAtUnchecked(il2cpp_array_size_t index)
	{
		return m_Items + index;
	}
	inline void SetAtUnchecked(il2cpp_array_size_t index, Il2CppChar value)
	{
		m_Items[index] = value;
	}
};


// T Facebook.Unity.ComponentFactory::GetComponent<System.Object>(Facebook.Unity.ComponentFactory/IfNotExist)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* ComponentFactory_GetComponent_TisRuntimeObject_m2E16AB838B228E32DF5CA1DBF301A0A99BF99E9C_gshared (int32_t ___0_ifNotExist, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<System.Object>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared (JavaMethodCall_1_t27CF0C3D13B160B267BA993DED70243E1963EE60* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodCall`1<System.Object>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline (MethodCall_1_tBE309A8BE882A6A1F80E42A8EE1493B9CB4FBBC8* __this, FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9* ___0_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddNullablePrimitive<System.Int32Enum>(System.String,System.Nullable`1<T>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments_AddNullablePrimitive_TisInt32Enum_tCBAC8BA2BFF3A845FA599F303093BBBA374B6F0C_m62C60E8BC5B040E477603C5794C447A49E17EFF2_gshared (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, Nullable_1_t163D49A1147F217B7BD43BE8ACC8A5CC6B846D14 ___1_nullable, const RuntimeMethod* method) ;
// System.Boolean System.Linq.Enumerable::Any<System.Object>(System.Collections.Generic.IEnumerable`1<TSource>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Enumerable_Any_TisRuntimeObject_m67CFBD544CF1D1C0C7E7457FDBDB81649DE26847_gshared (RuntimeObject* ___0_source, const RuntimeMethod* method) ;
// TSource System.Linq.Enumerable::First<System.Object>(System.Collections.Generic.IEnumerable`1<TSource>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* Enumerable_First_TisRuntimeObject_mEFECF1B8C3201589C5AF34176DCBF8DD926642D6_gshared (RuntimeObject* ___0_source, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddNullablePrimitive<System.Int32>(System.String,System.Nullable`1<T>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_gshared (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___1_nullable, const RuntimeMethod* method) ;
// System.Boolean System.Nullable`1<System.Single>::get_HasValue()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Nullable_1_get_HasValue_mC149B1C717AF506BBE8932F2C1DC86C378D17EA8_gshared_inline (Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75* __this, const RuntimeMethod* method) ;
// T System.Nullable`1<System.Single>::GetValueOrDefault()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Nullable_1_GetValueOrDefault_m068A148705ED1E215A5E85D18BA6852B192DA419_gshared_inline (Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddPrimative<System.Int32>(System.String,T)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC_gshared (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, int32_t ___1_value, const RuntimeMethod* method) ;
// System.Void System.Func`2<System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>,System.Object>::.ctor(System.Object,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Func_2__ctor_m60F64297108A01DFB5663C9BA121893957855907_gshared (Func_2_tF42287527472FA89789873F068A87C60A00EC7D3* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
// System.Collections.Generic.Dictionary`2<TKey,TElement> System.Linq.Enumerable::ToDictionary<System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>,System.Object,System.Object>(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>,System.Func`2<TSource,TElement>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Dictionary_2_t14FE4A752A83D53771C584E4C8D14E01F2AFD7BA* Enumerable_ToDictionary_TisKeyValuePair_2_tFC32D2507216293851350D29B64D79F950B55230_TisRuntimeObject_TisRuntimeObject_mFAD38355767A6BC98DB0AF76ADAB9AEDE1A401CB_gshared (RuntimeObject* ___0_source, Func_2_tF42287527472FA89789873F068A87C60A00EC7D3* ___1_keySelector, Func_2_tF42287527472FA89789873F068A87C60A00EC7D3* ___2_elementSelector, const RuntimeMethod* method) ;
// TKey System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>::get_Key()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* KeyValuePair_2_get_Key_mBD8EA7557C27E6956F2AF29DA3F7499B2F51A282_gshared_inline (KeyValuePair_2_tFC32D2507216293851350D29B64D79F950B55230* __this, const RuntimeMethod* method) ;
// TValue System.Collections.Generic.KeyValuePair`2<System.Object,System.Object>::get_Value()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* KeyValuePair_2_get_Value_mC6BD8075F9C9DDEF7B4D731E5C38EC19103988E7_gshared_inline (KeyValuePair_2_tFC32D2507216293851350D29B64D79F950B55230* __this, const RuntimeMethod* method) ;
// System.Void UnityEngine.Events.UnityAction`2<UnityEngine.SceneManagement.Scene,System.Int32Enum>::.ctor(System.Object,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void UnityAction_2__ctor_m7445B0F04ECB8542147C3C9B963A792140CFAD0A_gshared (UnityAction_2_tF47D82C7E3C3B118B409866D926435B55A0675BD* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddPrimative<System.Boolean>(System.String,T)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9_gshared (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, bool ___1_value, const RuntimeMethod* method) ;
// System.String Facebook.Unity.CallbackManager::AddFacebookDelegate<System.Object>(Facebook.Unity.FacebookDelegate`1<T>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* CallbackManager_AddFacebookDelegate_TisRuntimeObject_m06FC472C935576FDD955604D02F951D611997DA1_gshared (CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* __this, FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9* ___0_callback, const RuntimeMethod* method) ;
// System.Boolean System.Nullable`1<System.Int32Enum>::get_HasValue()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Nullable_1_get_HasValue_mB1F55188CDD50D6D725D41F55D2F2540CD15FB20_gshared_inline (Nullable_1_t163D49A1147F217B7BD43BE8ACC8A5CC6B846D14* __this, const RuntimeMethod* method) ;
// System.String System.Nullable`1<System.Int32Enum>::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Nullable_1_ToString_m3AEB3C73853E9F5C088E8A05EC8C27B722BDBE4D_gshared (Nullable_1_t163D49A1147F217B7BD43BE8ACC8A5CC6B846D14* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddList<System.Object>(System.String,System.Collections.Generic.IEnumerable`1<T>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments_AddList_TisRuntimeObject_mD72031331AC5437B40C6D0570A060B9271CFF369_gshared (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, RuntimeObject* ___1_list, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<System.Object>::.ctor(Facebook.Unity.Canvas.CanvasFacebook,System.String,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasUIMethodCall_1__ctor_m12C5E340F6FA46E90BED3201BB97BE4BEB0FB040_gshared (CanvasUIMethodCall_1_t4DF5B3B0E021839100A7F808C6A6A596BC1405C3* __this, CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* ___0_canvasImpl, String_t* ___1_methodName, String_t* ___2_callbackMethod, const RuntimeMethod* method) ;
// System.Void System.Collections.Generic.Dictionary`2<System.Object,System.Object>::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Dictionary_2__ctor_m5B32FBC624618211EB461D59CFBB10E987FD1329_gshared (Dictionary_2_t14FE4A752A83D53771C584E4C8D14E01F2AFD7BA* __this, const RuntimeMethod* method) ;
// System.Void System.Collections.Generic.Dictionary`2<System.Object,System.Object>::Add(TKey,TValue)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Dictionary_2_Add_m93FFFABE8FCE7FA9793F0915E2A8842C7CD0C0C1_gshared (Dictionary_2_t14FE4A752A83D53771C584E4C8D14E01F2AFD7BA* __this, RuntimeObject* ___0_key, RuntimeObject* ___1_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FacebookDelegate`1<System.Object>::Invoke(T)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void FacebookDelegate_1_Invoke_mBF6BA93033A7B72E328E2BA4C01FC55B970E123B_gshared_inline (FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9* __this, RuntimeObject* ___0_result, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Utilities/Callback`1<System.Object>::.ctor(System.Object,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Callback_1__ctor_m9AE1E70F081B722576F530FCCAF9AF1C672CA76C_gshared (Callback_1_t244FCFB1C42CF22CE5AF6954370376FF384F449A* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Utilities/Callback`1<System.Object>::Invoke(T)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Callback_1_Invoke_m8D352E98D69718836E0A8A72F8170BD075F846B5_gshared_inline (Callback_1_t244FCFB1C42CF22CE5AF6954370376FF384F449A* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) ;
// System.Boolean Facebook.Unity.Utilities::TryGetValue<System.Object>(System.Collections.Generic.IDictionary`2<System.String,System.Object>,System.String,T&)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Utilities_TryGetValue_TisRuntimeObject_mFCB0A986B3D5F7A7BC16BB7A06D3639579533C12_gshared (RuntimeObject* ___0_dictionary, String_t* ___1_key, RuntimeObject** ___2_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FacebookDelegate`1<System.Object>::.ctor(System.Object,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void FacebookDelegate_1__ctor_m44F1504FE737AA983D8B198477F3B73757EB69B5_gshared (FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
// System.Void System.Collections.Generic.List`1<System.Object>::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void List_1__ctor_m7F078BB342729BDF11327FD89D7872265328F690_gshared (List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* __this, const RuntimeMethod* method) ;
// T UnityEngine.GameObject::AddComponent<System.Object>()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* GameObject_AddComponent_TisRuntimeObject_m69B93700FACCF372F5753371C6E8FB780800B824_gshared (GameObject_t76FEDD663AB33C991A9C9A23129337651094216F* __this, const RuntimeMethod* method) ;

// System.Void System.Object::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2 (RuntimeObject* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::set_NumEntries(System.Int32)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void NativeDict_set_NumEntries_m57353835AD86ED502693E4BE8DCE1E3265BD6B52_inline (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, int32_t ___0_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::set_Keys(System.String[])
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void NativeDict_set_Keys_m30955DED0955CCE506D1FC315F2F534BF1450D7D_inline (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___0_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::set_Values(System.String[])
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void NativeDict_set_Values_m2D082203061E2C96CD3F728152BB43361E3CD860_inline (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___0_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.MobileFacebookGameObject::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MobileFacebookGameObject__ctor_m21F7B347AB7787EBE63D37862A1BB5F191E9B17D (MobileFacebookGameObject_t105BCEF7BF0751B141C332580EE375559D10BB7C* __this, const RuntimeMethod* method) ;
// T Facebook.Unity.ComponentFactory::GetComponent<Facebook.Unity.Mobile.IOS.IOSFacebookGameObject>(Facebook.Unity.ComponentFactory/IfNotExist)
inline IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA* ComponentFactory_GetComponent_TisIOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA_m1537424BCE59DE26311F38D9FDC0B8F90AAEC688 (int32_t ___0_ifNotExist, const RuntimeMethod* method)
{
	return ((  IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA* (*) (int32_t, const RuntimeMethod*))ComponentFactory_GetComponent_TisRuntimeObject_m2E16AB838B228E32DF5CA1DBF301A0A99BF99E9C_gshared)(___0_ifNotExist, method);
}
// Facebook.Unity.IFacebookImplementation Facebook.Unity.FacebookGameObject::get_Facebook()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* FacebookGameObject_get_Facebook_m8F6DC9F80E732D237D7F858FB1FC5D448071D137_inline (FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebook::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void IOSFacebook__ctor_m09F8C5E4063FAB2C5C2084B32F924630161331AB (IOSFacebook_tC5DD1DAB3274516F7194A166C4C9D5C2900A99F4* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FacebookGameObject::set_Facebook(Facebook.Unity.IFacebookImplementation)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void FacebookGameObject_set_Facebook_mEDCEAC5301992E16A962C3993F25C151F6251F1C_inline (FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B* __this, RuntimeObject* ___0_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FB/CompiledFacebookLoader::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CompiledFacebookLoader__ctor_m0CEF02AE4B19FCB945491FD9DE8B4CAB0CB4F227 (CompiledFacebookLoader_tDDF38F9F03C32FC27FB7546387BA7B9DBDCF5089* __this, const RuntimeMethod* method) ;
// Facebook.Unity.Mobile.Android.IAndroidWrapper Facebook.Unity.Mobile.Android.AndroidFacebook::GetAndroidWrapper()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* AndroidFacebook_GetAndroidWrapper_mFB879F02BAD046A4C58F0A4AFCADE56BB98673A9 (const RuntimeMethod* method) ;
// System.Void Facebook.Unity.CallbackManager::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CallbackManager__ctor_mBDDC9E4FCC6A9A0CCDE01755DC232BFCA66D4BA8 (CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::.ctor(Facebook.Unity.Mobile.Android.IAndroidWrapper,Facebook.Unity.CallbackManager)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook__ctor_m9AE45755787625750D7BB161746CCF187F2D327B (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, RuntimeObject* ___0_androidWrapper, CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* ___1_callbackManager, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.MobileFacebook::.ctor(Facebook.Unity.CallbackManager)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MobileFacebook__ctor_m6A029489BABC6CB3915882605CC3F8E4EFD3DFD1 (MobileFacebook_tFB7FCB79FF54D81C7EB30A5D40786FAB1DBFB21E* __this, CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* ___0_callbackManager, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::set_KeyHash(System.String)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void AndroidFacebook_set_KeyHash_m955883E7FF2093A8219358F222D292B3E556FED1_inline (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_value, const RuntimeMethod* method) ;
// System.String System.Boolean::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Boolean_ToString_m6646C8026B1DF381A1EE8CD13549175E9703CC63 (bool* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::CallFB(System.String,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_CallFB_mBFD340DDC8C5FD7AD5B9498EC31A03C371448889 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_method, String_t* ___1_args, const RuntimeMethod* method) ;
// System.String Facebook.Unity.Constants::get_UnitySDKUserAgentSuffixLegacy()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Constants_get_UnitySDKUserAgentSuffixLegacy_mC9A4AA341CF075CD6CA1B6D37E63BCB56A40BB60 (const RuntimeMethod* method) ;
// System.String System.String::Format(System.String,System.Object[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* String_Format_m918500C1EFB475181349A79989BB79BB36102894 (String_t* ___0_format, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___1_args, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FacebookBase::Init(Facebook.Unity.InitDelegate)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void FacebookBase_Init_m20FB2F18FDA2C3A4FA3EAD6C734EEFEAA5CF5F6A_inline (FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* __this, InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F* ___0_onInitComplete, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974 (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddString(System.String,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, String_t* ___1_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06 (JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Boolean UnityEngine.Debug::get_isDebugBuild()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool Debug_get_isDebugBuild_m9277C4A9591F7E1D8B76340B4CAE5EA33D63AF01 (const RuntimeMethod* method) ;
// System.Void UnityEngine.Debug::Log(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Debug_Log_m87A9A3C761FF5C43ED8A53B16190A53D08F818BB (RuntimeObject* ___0_message, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddCommaSeparatedList(System.String,System.Collections.Generic.IEnumerable`1<System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments_AddCommaSeparatedList_m4E3E59B109499EC3D6810CBEE16437DB193E0C38 (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, RuntimeObject* ___1_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ILoginResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mEDBCDC346A8A1302DA0A03038DF6159D0DA5FB9E (JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ILoginResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m6AE07A293D1DB63C07A09D30AC74C42874098633_inline (MethodCall_1_tFC828FE9EEAFBD29ACB6560A77095DF9CC02959F* __this, FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_tFC828FE9EEAFBD29ACB6560A77095DF9CC02959F*, FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.FacebookBase::LogOut()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void FacebookBase_LogOut_m2FFCD340816DE9D0B0AD32D5DDF89DFED7C62751 (FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* __this, const RuntimeMethod* method) ;
// System.Boolean System.String::IsNullOrEmpty(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool String_IsNullOrEmpty_mEA9E3FB005AC28FE02E69FCF95A7B8456192B478 (String_t* ___0_value, const RuntimeMethod* method) ;
// System.Collections.Generic.IDictionary`2<System.String,System.String> Facebook.Unity.Utilities::ParseStringDictionaryFromString(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* Utilities_ParseStringDictionaryFromString_mC44A9B19967AC7A42BD9038BFB475AC8700A8AB5 (String_t* ___0_input, const RuntimeMethod* method) ;
// Facebook.Unity.UserAgeRange Facebook.Unity.UserAgeRange::AgeRangeFromDictionary(System.Collections.Generic.IDictionary`2<System.String,System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR UserAgeRange_t3074872BAC5CB213D6E7245C5C9407CA12B7321A* UserAgeRange_AgeRangeFromDictionary_mE6F627CE372837D202F09D5C8F08DCD8737F9809 (RuntimeObject* ___0_dictionary, const RuntimeMethod* method) ;
// Facebook.Unity.FBLocation Facebook.Unity.FBLocation::FromDictionary(System.String,System.Collections.Generic.IDictionary`2<System.String,System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* FBLocation_FromDictionary_mD50DFBAB5217A454D51D465E834AE81A2DE6CDAD (String_t* ___0_prefix, RuntimeObject* ___1_dictionary, const RuntimeMethod* method) ;
// System.String[] System.String::Split(System.Char[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* String_Split_m101D35FEC86371D2BB4E3480F6F896880093B2E9 (String_t* __this, CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB* ___0_separator, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Profile::.ctor(System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String,System.String[],System.String,Facebook.Unity.UserAgeRange,Facebook.Unity.FBLocation,Facebook.Unity.FBLocation,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Profile__ctor_mDDFECEFF520F5C549B21BF3AB352D47D2F7B999C (Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7* __this, String_t* ___0_userID, String_t* ___1_firstName, String_t* ___2_middleName, String_t* ___3_lastName, String_t* ___4_name, String_t* ___5_email, String_t* ___6_imageURL, String_t* ___7_linkURL, StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___8_friendIDs, String_t* ___9_birthday, UserAgeRange_t3074872BAC5CB213D6E7245C5C9407CA12B7321A* ___10_ageRange, FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* ___11_hometown, FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* ___12_location, String_t* ___13_gender, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.LoginStatusResult::.ctor(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LoginStatusResult__ctor_m9A4F408E6505B85F672717A3A4FB3F2221A645A4 (LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FacebookBase::ValidateAppRequestArgs(System.String,System.Nullable`1<Facebook.Unity.OGActionType>,System.String,System.Collections.Generic.IEnumerable`1<System.String>,System.Collections.Generic.IEnumerable`1<System.Object>,System.Collections.Generic.IEnumerable`1<System.String>,System.Nullable`1<System.Int32>,System.String,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppRequestResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void FacebookBase_ValidateAppRequestArgs_m90BA81706354BF100FBBCEDAD59FA567395266D8 (FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* __this, String_t* ___0_message, Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73 ___1_actionType, String_t* ___2_objectId, RuntimeObject* ___3_to, RuntimeObject* ___4_filters, RuntimeObject* ___5_excludeIds, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___6_maxRecipients, String_t* ___7_data, String_t* ___8_title, FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2* ___9_callback, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddNullablePrimitive<Facebook.Unity.OGActionType>(System.String,System.Nullable`1<T>)
inline void MethodArguments_AddNullablePrimitive_TisOGActionType_t779FA0C877B3CD5177F27123B2E9CB4CEB14EC2E_mF55BAB58959DD27895825BFF8DE9DCEEAC13DED7 (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73 ___1_nullable, const RuntimeMethod* method)
{
	((  void (*) (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*, String_t*, Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73, const RuntimeMethod*))MethodArguments_AddNullablePrimitive_TisInt32Enum_tCBAC8BA2BFF3A845FA599F303093BBBA374B6F0C_m62C60E8BC5B040E477603C5794C447A49E17EFF2_gshared)(__this, ___0_argumentName, ___1_nullable, method);
}
// System.Boolean System.Linq.Enumerable::Any<System.Object>(System.Collections.Generic.IEnumerable`1<TSource>)
inline bool Enumerable_Any_TisRuntimeObject_m67CFBD544CF1D1C0C7E7457FDBDB81649DE26847 (RuntimeObject* ___0_source, const RuntimeMethod* method)
{
	return ((  bool (*) (RuntimeObject*, const RuntimeMethod*))Enumerable_Any_TisRuntimeObject_m67CFBD544CF1D1C0C7E7457FDBDB81649DE26847_gshared)(___0_source, method);
}
// TSource System.Linq.Enumerable::First<System.Object>(System.Collections.Generic.IEnumerable`1<TSource>)
inline RuntimeObject* Enumerable_First_TisRuntimeObject_mEFECF1B8C3201589C5AF34176DCBF8DD926642D6 (RuntimeObject* ___0_source, const RuntimeMethod* method)
{
	return ((  RuntimeObject* (*) (RuntimeObject*, const RuntimeMethod*))Enumerable_First_TisRuntimeObject_mEFECF1B8C3201589C5AF34176DCBF8DD926642D6_gshared)(___0_source, method);
}
// System.Void Facebook.Unity.MethodArguments::AddNullablePrimitive<System.Int32>(System.String,System.Nullable`1<T>)
inline void MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___1_nullable, const RuntimeMethod* method)
{
	((  void (*) (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*, String_t*, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28, const RuntimeMethod*))MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_gshared)(__this, ___0_argumentName, ___1_nullable, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppRequestResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mF266633C897B9D87553E0CB9CF81EAE049A9A423 (JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IAppRequestResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m097F0A862D0980C829D48D394888D0370C9EFB85_inline (MethodCall_1_t1D2B62ADEB209FD37D3B0AED0788B5A36C325DED* __this, FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t1D2B62ADEB209FD37D3B0AED0788B5A36C325DED*, FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.MethodArguments::AddUri(System.String,System.Uri)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6 (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___1_uri, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IShareResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mCD8FA794A4409286C7AFB41CA358BD8620A1D230 (JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IShareResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_inline (MethodCall_1_t0D27E61246A32F94E6F80F665C505FC54279576C* __this, FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t0D27E61246A32F94E6F80F665C505FC54279576C*, FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAppLinkResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mC2486364FEAA45691A130BAFFF533BC1384E5BEF (JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IAppLinkResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m00241290CE194EF41B420201C9BC57834F005704_inline (MethodCall_1_t73261478565B64ABA19B127D15BD7DCCD81D6A06* __this, FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t73261478565B64ABA19B127D15BD7DCCD81D6A06*, FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Boolean System.Nullable`1<System.Single>::get_HasValue()
inline bool Nullable_1_get_HasValue_mC149B1C717AF506BBE8932F2C1DC86C378D17EA8_inline (Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75* __this, const RuntimeMethod* method)
{
	return ((  bool (*) (Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75*, const RuntimeMethod*))Nullable_1_get_HasValue_mC149B1C717AF506BBE8932F2C1DC86C378D17EA8_gshared_inline)(__this, method);
}
// T System.Nullable`1<System.Single>::GetValueOrDefault()
inline float Nullable_1_GetValueOrDefault_m068A148705ED1E215A5E85D18BA6852B192DA419_inline (Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75* __this, const RuntimeMethod* method)
{
	return ((  float (*) (Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75*, const RuntimeMethod*))Nullable_1_GetValueOrDefault_m068A148705ED1E215A5E85D18BA6852B192DA419_gshared_inline)(__this, method);
}
// System.Globalization.CultureInfo System.Globalization.CultureInfo::get_InvariantCulture()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* CultureInfo_get_InvariantCulture_mD1E96DC845E34B10F78CB744B0CB5D7D63CEB1E6 (const RuntimeMethod* method) ;
// System.String System.Single::ToString(System.IFormatProvider)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Single_ToString_m534852BD7949AA972435783D7B96D0FFB09F6D6A (float* __this, RuntimeObject* ___0_provider, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddDictionary(System.String,System.Collections.Generic.IDictionary`2<System.String,System.Object>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MethodArguments_AddDictionary_m1633D0C3C4E5262F1F846414317BEED6C32EB75C (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, RuntimeObject* ___1_dict, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m07934BE9727E716B67132D5648DCC2FD307D5281 (JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m4EB4144CB0D6BAAD051358C0C21EA8D28E43A4DB_inline (MethodCall_1_t1852FDCA7708C74BD9575219E6FC7F609E56BA8E* __this, FacebookDelegate_1_t0A787A64D3187E98A865A198DDF444C008C79F35* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t1852FDCA7708C74BD9575219E6FC7F609E56BA8E*, FacebookDelegate_1_t0A787A64D3187E98A865A198DDF444C008C79F35*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m9EDA60F8B74B5DE2F1D9781C1D09AE56F5F490FA (JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m7B3DF88D41AC203597412EF880B1442EA25112B4_inline (MethodCall_1_t28181D9D5D0204C879A034D9757AF1F63C3EC7AF* __this, FacebookDelegate_1_t55619BCDC758CB1C82277E6D8BA44ACA47C03DE2* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t28181D9D5D0204C879A034D9757AF1F63C3EC7AF*, FacebookDelegate_1_t55619BCDC758CB1C82277E6D8BA44ACA47C03DE2*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IMediaUploadResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m23975F7FD0FE3A43155D75E53A95085F6504F9CD (JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IMediaUploadResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m5ACC9D63CAC86405A106A94B35C11B8641FA2088_inline (MethodCall_1_tACE924324819B8E5DAC6615D0A058E16C5A7C729* __this, FacebookDelegate_1_t7CA00F6A27B3FE85139590EFEA03B7C7C0D4A66D* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_tACE924324819B8E5DAC6615D0A058E16C5A7C729*, FacebookDelegate_1_t7CA00F6A27B3FE85139590EFEA03B7C7C0D4A66D*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IIAPReadyResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m98326A81B2D27D3EF9182B178B0B3B026C6A41E6 (JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IIAPReadyResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m3AC34DDE1F2C7F926E3440F251C1519997E656B5_inline (MethodCall_1_t3129C45AEAB38F50384EF247D7ED9921171D1DF7* __this, FacebookDelegate_1_tA2A619ACCF137D8094955A556E78A66749C43F74* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t3129C45AEAB38F50384EF247D7ED9921171D1DF7*, FacebookDelegate_1_tA2A619ACCF137D8094955A556E78A66749C43F74*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ICatalogResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m88BEF0F791A57E9BAF9D3C26D3053BBD54F4BD8D (JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ICatalogResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m0300E7B325CFF8E837D2D5B77E3B160C53C1068F_inline (MethodCall_1_tA51B63B488D06F9DD4269E5D936844297AC2F717* __this, FacebookDelegate_1_tA8F51BBA2E7364881368E0CBED892F561162C32E* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_tA51B63B488D06F9DD4269E5D936844297AC2F717*, FacebookDelegate_1_tA8F51BBA2E7364881368E0CBED892F561162C32E*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchasesResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mAC2E2E15420B915144E3A7324B6F901123C5BC22 (JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchasesResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m490C4FB7E8194080AD5A53C8BA7F96134C1EED27_inline (MethodCall_1_t964EEAC4E9A2090141E286A38DD20AD5D31C42B3* __this, FacebookDelegate_1_t61D056F5D405CB3DF2F643449D9C03A64AB8E818* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t964EEAC4E9A2090141E286A38DD20AD5D31C42B3*, FacebookDelegate_1_t61D056F5D405CB3DF2F643449D9C03A64AB8E818*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPurchaseResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m25A837E2CB92362A09A93D1260F3D010966A513E (JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchaseResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_mFFB10B41B3F6834AAF0A0A2D1EB306A89AA8E4A1_inline (MethodCall_1_tE2E7E098CBA48E270C26249E671C3D59EB7D0C27* __this, FacebookDelegate_1_tBD274D4AED39A6A24A22981D38D3CBF447CF036E* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_tE2E7E098CBA48E270C26249E671C3D59EB7D0C27*, FacebookDelegate_1_tBD274D4AED39A6A24A22981D38D3CBF447CF036E*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IConsumePurchaseResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m5F7216810B33A4CCED764468B20F505B5449669D (JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IConsumePurchaseResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m1725F4044DF470B54304044677371FA010FCA055_inline (MethodCall_1_t170D11D6663ABDBA2714482029BC9107E6119F32* __this, FacebookDelegate_1_tF96C838D4977CEFDB874E77F7A56B1CFECF4D8DD* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t170D11D6663ABDBA2714482029BC9107E6119F32*, FacebookDelegate_1_tF96C838D4977CEFDB874E77F7A56B1CFECF4D8DD*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInitCloudGameResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m550304F60083DC63749F387B03DF9E7777F83265 (JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IInitCloudGameResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m80D0B98B0FF86073D4DE52A2FB8C56374851424A_inline (MethodCall_1_tFDD6DDBFBE6A9432424C4841D30F5018A40CFEDC* __this, FacebookDelegate_1_tB829CDA8266CAB15D1703F5904BB2F8592CA60E8* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_tFDD6DDBFBE6A9432424C4841D30F5018A40CFEDC*, FacebookDelegate_1_tB829CDA8266CAB15D1703F5904BB2F8592CA60E8*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.MethodArguments::AddPrimative<System.Int32>(System.String,T)
inline void MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, int32_t ___1_value, const RuntimeMethod* method)
{
	((  void (*) (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*, String_t*, int32_t, const RuntimeMethod*))MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC_gshared)(__this, ___0_argumentName, ___1_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m1A91A6D8302126249B270F478B851AE405105FEE (JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m33FD8977CC0AFF79A6EBA5E2DE85109BDB68ED8F_inline (MethodCall_1_t619F8AD0F81AEF389F5C6063A4C1F8C06036B029* __this, FacebookDelegate_1_t623E520D5B99523D410AEE72AB9FF4901DC356AC* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t619F8AD0F81AEF389F5C6063A4C1F8C06036B029*, FacebookDelegate_1_t623E520D5B99523D410AEE72AB9FF4901DC356AC*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IInterstitialAdResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m82480001BC5DA6A73E94BC8C41F4296473A9141F (JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IInterstitialAdResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_mCE2EA397F7DFB68974A15A329A01050C09E92A61_inline (MethodCall_1_t7E8B67FDC468ABDD86D3E481B50034289CB7279D* __this, FacebookDelegate_1_t6BAE034F2CC3270BFC282A9D0DEB58CC5F91C265* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t7E8B67FDC468ABDD86D3E481B50034289CB7279D*, FacebookDelegate_1_t6BAE034F2CC3270BFC282A9D0DEB58CC5F91C265*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IRewardedVideoResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mF67A248E040456EE62DDE37D0E7A8BE10AFFD665 (JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IRewardedVideoResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m754DB9950904B1CCB5076A82982D93946AF22BE3_inline (MethodCall_1_tDFA97C21B6B2B2BC255F28E227D0F5FA5829F686* __this, FacebookDelegate_1_t9C5748E8AC36242F3F03171AA1E6306E60860ACD* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_tDFA97C21B6B2B2BC255F28E227D0F5FA5829F686*, FacebookDelegate_1_t9C5748E8AC36242F3F03171AA1E6306E60860ACD*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IPayloadResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mA93CFE9895EE4033CEE85613734423E1E8870310 (JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IPayloadResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m7C7EFBA28D827AB9DEE7946EBD20C87AD1EB44C5_inline (MethodCall_1_tDD7FB7C96BB6551BC13B0499833326C4B55CB468* __this, FacebookDelegate_1_tB6B04FA63685B9A0D41AB08E3E5BC96A72D7A1D7* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_tDD7FB7C96BB6551BC13B0499833326C4B55CB468*, FacebookDelegate_1_tB6B04FA63685B9A0D41AB08E3E5BC96A72D7A1D7*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.String System.Int32::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Int32_ToString_m030E01C24E294D6762FB0B6F37CB541581F55CA5 (int32_t* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ISessionScoreResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mCD10E054583159BC1950612666A16FD33761CBE4 (JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ISessionScoreResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_mE524DA942B4350B09ECDF58D3FA0D6D3765CAE4D_inline (MethodCall_1_t3A25163740B62335265043F26060847249D6C047* __this, FacebookDelegate_1_t610D359C8E47669CC0D9F0B6CA9DF9240BF80267* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t3A25163740B62335265043F26060847249D6C047*, FacebookDelegate_1_t610D359C8E47669CC0D9F0B6CA9DF9240BF80267*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentScoreResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mBC1E3309CE7ADE92E369530993D7A36F5EFEF649 (JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentScoreResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m695883E994C52B5CB6C0AB3939ED902B2F1455A5_inline (MethodCall_1_tACF312248F1E681E1DA6AF486A149C37756A81E3* __this, FacebookDelegate_1_t474682078C474498C8D4805F13E9077763B39255* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_tACF312248F1E681E1DA6AF486A149C37756A81E3*, FacebookDelegate_1_t474682078C474498C8D4805F13E9077763B39255*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.ITournamentResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_m633D3D2232A91D7FBE7FA9F6E87212EC1F47708C (JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m31B32945ADD64F439F695CD2B613934BEFCC15A8_inline (MethodCall_1_tAE6A4425ED88803929C6E6458A9EF1A83135B315* __this, FacebookDelegate_1_tD9C9BBFE6AD0931C935E391D858C37BEBF14BBA3* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_tAE6A4425ED88803929C6E6458A9EF1A83135B315*, FacebookDelegate_1_tD9C9BBFE6AD0931C935E391D858C37BEBF14BBA3*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.String>::.ctor(System.Object,System.IntPtr)
inline void Func_2__ctor_m48BD5538630AB90CAACF2ADC165985AB743A6C30 (Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088*, RuntimeObject*, intptr_t, const RuntimeMethod*))Func_2__ctor_m60F64297108A01DFB5663C9BA121893957855907_gshared)(__this, ___0_object, ___1_method, method);
}
// System.Void System.Func`2<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.Object>::.ctor(System.Object,System.IntPtr)
inline void Func_2__ctor_m554C9E1DBFF27492E948AA1DBB4446038F0EB728 (Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF*, RuntimeObject*, intptr_t, const RuntimeMethod*))Func_2__ctor_m60F64297108A01DFB5663C9BA121893957855907_gshared)(__this, ___0_object, ___1_method, method);
}
// System.Collections.Generic.Dictionary`2<TKey,TElement> System.Linq.Enumerable::ToDictionary<System.Collections.Generic.KeyValuePair`2<System.String,System.String>,System.String,System.Object>(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TKey>,System.Func`2<TSource,TElement>)
inline Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* Enumerable_ToDictionary_TisKeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A_TisString_t_TisRuntimeObject_m1565E67B7F63889DEE95110A76BCB790092DD21B (RuntimeObject* ___0_source, Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* ___1_keySelector, Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* ___2_elementSelector, const RuntimeMethod* method)
{
	return ((  Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* (*) (RuntimeObject*, Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088*, Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF*, const RuntimeMethod*))Enumerable_ToDictionary_TisKeyValuePair_2_tFC32D2507216293851350D29B64D79F950B55230_TisRuntimeObject_TisRuntimeObject_mFAD38355767A6BC98DB0AF76ADAB9AEDE1A401CB_gshared)(___0_source, ___1_keySelector, ___2_elementSelector, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/JavaMethodCall`1<Facebook.Unity.IOpenAppStoreResult>::.ctor(Facebook.Unity.Mobile.Android.AndroidFacebook,System.String)
inline void JavaMethodCall_1__ctor_mD281D9CF6AA50E4AF6D86C8046B972E4759C82E0 (JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5* __this, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* ___0_androidImpl, String_t* ___1_methodName, const RuntimeMethod* method)
{
	((  void (*) (JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5*, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*, String_t*, const RuntimeMethod*))JavaMethodCall_1__ctor_mD9C504594FB89844E3BF1172F60F46C71C405C57_gshared)(__this, ___0_androidImpl, ___1_methodName, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IOpenAppStoreResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m37167CE00F34CC6A9A24918CB34160CB9ECA017E_inline (MethodCall_1_t87E54F439C1534715A53A604C0EAE5850B55120D* __this, FacebookDelegate_1_t5E457BF6B03D4AD72D97F1848A0612B675747CFD* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t87E54F439C1534715A53A604C0EAE5850B55120D*, FacebookDelegate_1_t5E457BF6B03D4AD72D97F1848A0612B675747CFD*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.String System.Enum::ToString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Enum_ToString_m946B0B83C4470457D0FF555D862022C72BB55741 (RuntimeObject* __this, const RuntimeMethod* method) ;
// System.Reflection.Assembly System.Reflection.Assembly::Load(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Assembly_t* Assembly_Load_mC42733BACCA273EEAA32A341CBF53722A44DCC90 (String_t* ___0_assemblyString, const RuntimeMethod* method) ;
// System.Object System.Activator::CreateInstance(System.Type)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* Activator_CreateInstance_mFF030428C64FDDFACC74DFAC97388A1C628BFBCF (Type_t* ___0_type, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__ctor_m6FA067A84B1AE0399E7ED35538573D3678CC0B38 (U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* __this, const RuntimeMethod* method) ;
// TKey System.Collections.Generic.KeyValuePair`2<System.String,System.String>::get_Key()
inline String_t* KeyValuePair_2_get_Key_m654BCCAE2F20CB11D8E8C2D2C886A0C8A13EB1C4_inline (KeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A* __this, const RuntimeMethod* method)
{
	return ((  String_t* (*) (KeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A*, const RuntimeMethod*))KeyValuePair_2_get_Key_mBD8EA7557C27E6956F2AF29DA3F7499B2F51A282_gshared_inline)(__this, method);
}
// TValue System.Collections.Generic.KeyValuePair`2<System.String,System.String>::get_Value()
inline String_t* KeyValuePair_2_get_Value_m7345512A32CB4DCAA0643050B18DC8DCD71B927A_inline (KeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A* __this, const RuntimeMethod* method)
{
	return ((  String_t* (*) (KeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A*, const RuntimeMethod*))KeyValuePair_2_get_Value_mC6BD8075F9C9DDEF7B4D731E5C38EC19103988E7_gshared_inline)(__this, method);
}
// System.Void Facebook.Unity.CodelessIAPAutoLog::addListenerToIAPButtons(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CodelessIAPAutoLog_addListenerToIAPButtons_mF59A2E9C09D7A9C807725DCEDB909050ECFF0F7F (RuntimeObject* ___0_listenerObject, const RuntimeMethod* method) ;
// System.Void UnityEngine.Events.UnityAction`2<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>::.ctor(System.Object,System.IntPtr)
inline void UnityAction_2__ctor_m0E0C01B7056EB1CB1E6C6F4FC457EBCA3F6B0041 (UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A*, RuntimeObject*, intptr_t, const RuntimeMethod*))UnityAction_2__ctor_m7445B0F04ECB8542147C3C9B963A792140CFAD0A_gshared)(__this, ___0_object, ___1_method, method);
}
// System.Void UnityEngine.SceneManagement.SceneManager::add_sceneLoaded(UnityEngine.Events.UnityAction`2<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void SceneManager_add_sceneLoaded_m14BEBCC5E4A8DD2C806A48D79A4773315CB434C6 (UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A* ___0_value, const RuntimeMethod* method) ;
// System.Void UnityEngine.SceneManagement.SceneManager::remove_sceneLoaded(UnityEngine.Events.UnityAction`2<UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void SceneManager_remove_sceneLoaded_m72A7C2A1B8EF1C21A208A9A015375577768B3978 (UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A* ___0_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.CodelessIAPAutoLog::handlePurchaseCompleted(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CodelessIAPAutoLog_handlePurchaseCompleted_m06FA6E299411CFD7DB741AD50FBBC4F6F3E77CC4 (RuntimeObject* ___0_data, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.ResultContainer::.ctor(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ResultContainer__ctor_mAD6B7ADD48D8244E67DEC442276642A1DB0EB74C (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* __this, String_t* ___0_result, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::OnLoginStatusRetrieved(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_OnLoginStatusRetrieved_m65A558404816C7726AD1B59A65F8E35BD00A5F78 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) ;
// T Facebook.Unity.ComponentFactory::GetComponent<Facebook.Unity.Mobile.Android.AndroidFacebookGameObject>(Facebook.Unity.ComponentFactory/IfNotExist)
inline AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* ComponentFactory_GetComponent_TisAndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378_mAE5A1A01D902A79AC16E3656B097F207B8C4C9EC (int32_t ___0_ifNotExist, const RuntimeMethod* method)
{
	return ((  AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* (*) (int32_t, const RuntimeMethod*))ComponentFactory_GetComponent_TisRuntimeObject_m2E16AB838B228E32DF5CA1DBF301A0A99BF99E9C_gshared)(___0_ifNotExist, method);
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook__ctor_m4C9297CE471F88A30A971963689FAE8332984011 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, const RuntimeMethod* method) ;
// Facebook.Unity.Canvas.ICanvasJSWrapper Facebook.Unity.Canvas.CanvasFacebook::GetCanvasJSWrapper()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* CanvasFacebook_GetCanvasJSWrapper_mCAF1873B89014661AA22463DB1FCF2FFA5FE3CFD (const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Canvas.CanvasFacebook::.ctor(Facebook.Unity.Canvas.ICanvasJSWrapper,Facebook.Unity.CallbackManager)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook__ctor_m3ADD98C9D62A49DFCE51D16F6862FB80179E85E0 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, RuntimeObject* ___0_canvasJSWrapper, CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* ___1_callbackManager, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FacebookBase::.ctor(Facebook.Unity.CallbackManager)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void FacebookBase__ctor_m91D39F3C0A029D203CF6A8E08546742B1D7A5AFA (FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* __this, CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* ___0_callbackManager, const RuntimeMethod* method) ;
// Facebook.Unity.FacebookUnityPlatform Facebook.Unity.Constants::get_CurrentPlatform()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Constants_get_CurrentPlatform_mE6140F488BC2C82EC6FB0BE87184B6A8869050F8 (const RuntimeMethod* method) ;
// System.String System.String::Format(System.IFormatProvider,System.String,System.Object[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* String_Format_m447B585713E5EB3EBF5D9D0710706D01E8A56D75 (RuntimeObject* ___0_provider, String_t* ___1_format, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___2_args, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FacebookLogger::Warn(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void FacebookLogger_Warn_mDAD4A3F15FE52EF33DDC7A7B5639E04F057509D9 (String_t* ___0_msg, const RuntimeMethod* method) ;
// System.String Facebook.Unity.FacebookBase::get_SDKUserAgent()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* FacebookBase_get_SDKUserAgent_mDEA68FD4E4987F897BBAE09D4144B0159FEC408C (FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* __this, const RuntimeMethod* method) ;
// System.String Facebook.Unity.FacebookSdkVersion::get_Build()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* FacebookSdkVersion_get_Build_mBCD0ADAF8A500F19A9E9B15E820FB3C7726F598E (const RuntimeMethod* method) ;
// System.String Facebook.Unity.Utilities::GetUserAgent(System.String,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Utilities_GetUserAgent_m338A5F74F5F5125C3D5D03DBC2A722CC0B8BC7D1 (String_t* ___0_productName, String_t* ___1_productVersion, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.MethodArguments::AddPrimative<System.Boolean>(System.String,T)
inline void MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9 (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, bool ___1_value, const RuntimeMethod* method)
{
	((  void (*) (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*, String_t*, bool, const RuntimeMethod*))MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9_gshared)(__this, ___0_argumentName, ___1_value, method);
}
// System.String Facebook.Unity.FB::get_GraphApiVersion()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* FB_get_GraphApiVersion_m234E4C9258DE0F012E1A78D5A4DED1DE8D6D5198_inline (const RuntimeMethod* method) ;
// System.String Facebook.Unity.MethodArguments::ToJsonString()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* MethodArguments_ToJsonString_m806D99FEC9348537F888D9D2DA012720952F9CF7 (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, const RuntimeMethod* method) ;
// Facebook.Unity.CallbackManager Facebook.Unity.FacebookBase::get_CallbackManager()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* FacebookBase_get_CallbackManager_m6C4BEBEF920CD139CF777D45E0E924829E4CF57F_inline (FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* __this, const RuntimeMethod* method) ;
// System.String Facebook.Unity.CallbackManager::AddFacebookDelegate<Facebook.Unity.ILoginResult>(Facebook.Unity.FacebookDelegate`1<T>)
inline String_t* CallbackManager_AddFacebookDelegate_TisILoginResult_t809F9817555CF3FF1F2C11154D110DB3F55C07ED_m8D256EDC847AB71366C71A4DA1FA1B3A51E782AC (CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* __this, FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* ___0_callback, const RuntimeMethod* method)
{
	return ((  String_t* (*) (CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749*, FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D*, const RuntimeMethod*))CallbackManager_AddFacebookDelegate_TisRuntimeObject_m06FC472C935576FDD955604D02F951D611997DA1_gshared)(__this, ___0_callback, method);
}
// System.Boolean System.Nullable`1<Facebook.Unity.OGActionType>::get_HasValue()
inline bool Nullable_1_get_HasValue_m3C0F9BCB83ED49443257921B53C3AC3A95FEDC63_inline (Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73* __this, const RuntimeMethod* method)
{
	return ((  bool (*) (Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73*, const RuntimeMethod*))Nullable_1_get_HasValue_mB1F55188CDD50D6D725D41F55D2F2540CD15FB20_gshared_inline)(__this, method);
}
// System.String System.Nullable`1<Facebook.Unity.OGActionType>::ToString()
inline String_t* Nullable_1_ToString_mF981686677572249978468566375A4C296C6B97A (Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73* __this, const RuntimeMethod* method)
{
	return ((  String_t* (*) (Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73*, const RuntimeMethod*))Nullable_1_ToString_m3AEB3C73853E9F5C088E8A05EC8C27B722BDBE4D_gshared)(__this, method);
}
// System.Void Facebook.Unity.MethodArguments::AddList<System.Object>(System.String,System.Collections.Generic.IEnumerable`1<T>)
inline void MethodArguments_AddList_TisRuntimeObject_mD72031331AC5437B40C6D0570A060B9271CFF369 (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, RuntimeObject* ___1_list, const RuntimeMethod* method)
{
	((  void (*) (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*, String_t*, RuntimeObject*, const RuntimeMethod*))MethodArguments_AddList_TisRuntimeObject_mD72031331AC5437B40C6D0570A060B9271CFF369_gshared)(__this, ___0_argumentName, ___1_list, method);
}
// System.Void Facebook.Unity.MethodArguments::AddList<System.String>(System.String,System.Collections.Generic.IEnumerable`1<T>)
inline void MethodArguments_AddList_TisString_t_mBBEF6565354D6AE11E537FC132BC332546C22099 (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* __this, String_t* ___0_argumentName, RuntimeObject* ___1_list, const RuntimeMethod* method)
{
	((  void (*) (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*, String_t*, RuntimeObject*, const RuntimeMethod*))MethodArguments_AddList_TisRuntimeObject_mD72031331AC5437B40C6D0570A060B9271CFF369_gshared)(__this, ___0_argumentName, ___1_list, method);
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IAppRequestResult>::.ctor(Facebook.Unity.Canvas.CanvasFacebook,System.String,System.String)
inline void CanvasUIMethodCall_1__ctor_m3260CC061DE681C15F9551F21DBB0AB1914AD08D (CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE* __this, CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* ___0_canvasImpl, String_t* ___1_methodName, String_t* ___2_callbackMethod, const RuntimeMethod* method)
{
	((  void (*) (CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE*, CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1*, String_t*, String_t*, const RuntimeMethod*))CanvasUIMethodCall_1__ctor_m12C5E340F6FA46E90BED3201BB97BE4BEB0FB040_gshared)(__this, ___0_canvasImpl, ___1_methodName, ___2_callbackMethod, method);
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IShareResult>::.ctor(Facebook.Unity.Canvas.CanvasFacebook,System.String,System.String)
inline void CanvasUIMethodCall_1__ctor_m09ADE50A6A66328C007485BF17C322DE3149D2CD (CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052* __this, CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* ___0_canvasImpl, String_t* ___1_methodName, String_t* ___2_callbackMethod, const RuntimeMethod* method)
{
	((  void (*) (CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052*, CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1*, String_t*, String_t*, const RuntimeMethod*))CanvasUIMethodCall_1__ctor_m12C5E340F6FA46E90BED3201BB97BE4BEB0FB040_gshared)(__this, ___0_canvasImpl, ___1_methodName, ___2_callbackMethod, method);
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::PayImpl(System.String,System.String,System.String,System.Int32,System.Nullable`1<System.Int32>,System.Nullable`1<System.Int32>,System.String,System.String,System.String,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_PayImpl_m6559E97A2444BFABD5E6B5D4B8D14FB5DD984069 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, String_t* ___0_product, String_t* ___1_productId, String_t* ___2_action, int32_t ___3_quantity, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___4_quantityMin, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___5_quantityMax, String_t* ___6_requestId, String_t* ___7_pricepointId, String_t* ___8_testCurrency, String_t* ___9_developerPayload, FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B* ___10_callback, const RuntimeMethod* method) ;
// System.Void System.Collections.Generic.Dictionary`2<System.String,System.Object>::.ctor()
inline void Dictionary_2__ctor_mC4F3DF292BAD88F4BF193C49CD689FAEBC4570A9 (Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* __this, const RuntimeMethod* method)
{
	((  void (*) (Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710*, const RuntimeMethod*))Dictionary_2__ctor_m5B32FBC624618211EB461D59CFBB10E987FD1329_gshared)(__this, method);
}
// System.Void System.Collections.Generic.Dictionary`2<System.String,System.Object>::Add(TKey,TValue)
inline void Dictionary_2_Add_m5875DF2ACE933D734119C088B2E7C9C63F49B443 (Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* __this, String_t* ___0_key, RuntimeObject* ___1_value, const RuntimeMethod* method)
{
	((  void (*) (Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710*, String_t*, RuntimeObject*, const RuntimeMethod*))Dictionary_2_Add_m93FFFABE8FCE7FA9793F0915E2A8842C7CD0C0C1_gshared)(__this, ___0_key, ___1_value, method);
}
// System.Void Facebook.Unity.ResultContainer::.ctor(System.Collections.Generic.IDictionary`2<System.String,System.Object>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ResultContainer__ctor_m83395A80E1E9600322982ECDA27159717BBF44E7 (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* __this, RuntimeObject* ___0_dictionary, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.AppLinkResult::.ctor(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AppLinkResult__ctor_mB1D124600541A7821BCE9FCB190FDABA5C46FF59 (AppLinkResult_t7B3A5BB1C71DF6D48BBC7B508AE365E8D4A79143* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppLinkResult>::Invoke(T)
inline void FacebookDelegate_1_Invoke_m82AD0F3529DD1BA0DC1E547A1AC3C84DD1BC83A5_inline (FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D* __this, RuntimeObject* ___0_result, const RuntimeMethod* method)
{
	((  void (*) (FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D*, RuntimeObject*, const RuntimeMethod*))FacebookDelegate_1_Invoke_mBF6BA93033A7B72E328E2BA4C01FC55B970E123B_gshared_inline)(__this, ___0_result, method);
}
// System.String Facebook.MiniJSON.Json::Serialize(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Json_Serialize_mB946D73EA18C733AA78049E62EFD0D8BD04328CB (RuntimeObject* ___0_obj, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer>::.ctor(System.Object,System.IntPtr)
inline void Callback_1__ctor_mE4AFB946F865A019EC8E67BFE4552B5F7FC4DA51 (Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92*, RuntimeObject*, intptr_t, const RuntimeMethod*))Callback_1__ctor_m9AE1E70F081B722576F530FCCAF9AF1C672CA76C_gshared)(__this, ___0_object, ___1_method, method);
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::FormatAuthResponse(Facebook.Unity.ResultContainer,Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_FormatAuthResponse_mF113AE6F0CECF177B251B4989E62E404E69BB0BF (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_result, Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* ___1_callback, const RuntimeMethod* method) ;
// System.Void System.NotImplementedException::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NotImplementedException__ctor_mDAB47BC6BD0E342E8F2171E5CABE3E67EA049F1C (NotImplementedException_t6366FE4DCF15094C51F4833B91A2AE68D4DA90E8* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.PayResult::.ctor(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void PayResult__ctor_mD2C8086EE7A0649161C6E217605A1AB933E86B95 (PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.CallbackManager::OnFacebookResponse(Facebook.Unity.IInternalResult)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CallbackManager_OnFacebookResponse_mA506CEDF0220B6EA8F5D36F639E1840F15E255A6 (CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* __this, RuntimeObject* ___0_result, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.AppRequestResult::.ctor(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AppRequestResult__ctor_m4595618FC82A0143E3A8799D4252798FF0AD643D (AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.ShareResult::.ctor(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ShareResult__ctor_m5AAAC8595A4E1839AEDE81113879546BBE11458F (ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.HideUnityDelegate::Invoke(System.Boolean)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void HideUnityDelegate_Invoke_m84D79F9CD775257293DF8E35297F43835E18CD71_inline (HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE* __this, bool ___0_isUnityShown, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Canvas.CanvasFacebook/<>c__DisplayClass47_0::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__DisplayClass47_0__ctor_m8D4983F5B36F90967DDFDD0051DBDD7BE6E39DE7 (U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* __this, const RuntimeMethod* method) ;
// System.Collections.Generic.IDictionary`2<System.String,System.Object> Facebook.Unity.ResultContainer::get_ResultDictionary()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer>::Invoke(T)
inline void Callback_1_Invoke_mFD8528526031568E661F81643EF709689D3884B0_inline (Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_obj, const RuntimeMethod* method)
{
	((  void (*) (Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92*, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B*, const RuntimeMethod*))Callback_1_Invoke_m8D352E98D69718836E0A8A72F8170BD075F846B5_gshared_inline)(__this, ___0_obj, method);
}
// System.Boolean Facebook.Unity.Utilities::TryGetValue<System.Collections.Generic.IDictionary`2<System.String,System.Object>>(System.Collections.Generic.IDictionary`2<System.String,System.Object>,System.String,T&)
inline bool Utilities_TryGetValue_TisIDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_mF4F1612A6CC71E09B0A940E7B840C94637D8BB97 (RuntimeObject* ___0_dictionary, String_t* ___1_key, RuntimeObject** ___2_value, const RuntimeMethod* method)
{
	return ((  bool (*) (RuntimeObject*, String_t*, RuntimeObject**, const RuntimeMethod*))Utilities_TryGetValue_TisRuntimeObject_mFCB0A986B3D5F7A7BC16BB7A06D3639579533C12_gshared)(___0_dictionary, ___1_key, ___2_value, method);
}
// TKey System.Collections.Generic.KeyValuePair`2<System.String,System.Object>::get_Key()
inline String_t* KeyValuePair_2_get_Key_mA64FF29A08423140758B0276333D1A89C71B793A_inline (KeyValuePair_2_tBEE55F2A4574C64393155C322376FD98C7BFC7B9* __this, const RuntimeMethod* method)
{
	return ((  String_t* (*) (KeyValuePair_2_tBEE55F2A4574C64393155C322376FD98C7BFC7B9*, const RuntimeMethod*))KeyValuePair_2_get_Key_mBD8EA7557C27E6956F2AF29DA3F7499B2F51A282_gshared_inline)(__this, method);
}
// TValue System.Collections.Generic.KeyValuePair`2<System.String,System.Object>::get_Value()
inline RuntimeObject* KeyValuePair_2_get_Value_m2052BF44A3FDE623D98B0E6B6E227B2900034235_inline (KeyValuePair_2_tBEE55F2A4574C64393155C322376FD98C7BFC7B9* __this, const RuntimeMethod* method)
{
	return ((  RuntimeObject* (*) (KeyValuePair_2_tBEE55F2A4574C64393155C322376FD98C7BFC7B9*, const RuntimeMethod*))KeyValuePair_2_get_Value_mC6BD8075F9C9DDEF7B4D731E5C38EC19103988E7_gshared_inline)(__this, method);
}
// System.Void System.Collections.Generic.Dictionary`2<System.String,System.String>::.ctor()
inline void Dictionary_2__ctor_m768E076F1E804CE4959F4E71D3E6A9ADE2F55052 (Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* __this, const RuntimeMethod* method)
{
	((  void (*) (Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83*, const RuntimeMethod*))Dictionary_2__ctor_m5B32FBC624618211EB461D59CFBB10E987FD1329_gshared)(__this, method);
}
// System.Void System.Collections.Generic.Dictionary`2<System.String,System.String>::Add(TKey,TValue)
inline void Dictionary_2_Add_mC78C20D5901C87AAC38F37C906FAB6946BDE5F13 (Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* __this, String_t* ___0_key, String_t* ___1_value, const RuntimeMethod* method)
{
	((  void (*) (Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83*, String_t*, String_t*, const RuntimeMethod*))Dictionary_2_Add_m93FFFABE8FCE7FA9793F0915E2A8842C7CD0C0C1_gshared)(__this, ___0_key, ___1_value, method);
}
// System.Void Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGraphResult>::.ctor(System.Object,System.IntPtr)
inline void FacebookDelegate_1__ctor_m9A4C43BB75B9BA8CAE0A29F54DFFC9F4A7EE1F27 (FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB*, RuntimeObject*, intptr_t, const RuntimeMethod*))FacebookDelegate_1__ctor_m44F1504FE737AA983D8B198477F3B73757EB69B5_gshared)(__this, ___0_object, ___1_method, method);
}
// System.Void Facebook.Unity.FB::API(System.String,Facebook.Unity.HttpMethod,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGraphResult>,System.Collections.Generic.IDictionary`2<System.String,System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void FB_API_m99EA09DCB400B85BC92B3E9E887D2D934B999B14 (String_t* ___0_query, int32_t ___1_method, FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB* ___2_callback, RuntimeObject* ___3_formData, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Canvas.CanvasFacebook/CanvasUIMethodCall`1<Facebook.Unity.IPayResult>::.ctor(Facebook.Unity.Canvas.CanvasFacebook,System.String,System.String)
inline void CanvasUIMethodCall_1__ctor_m1C9CF2D82C5B3A9F345A89045A43FF89270325F9 (CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6* __this, CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* ___0_canvasImpl, String_t* ___1_methodName, String_t* ___2_callbackMethod, const RuntimeMethod* method)
{
	((  void (*) (CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6*, CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1*, String_t*, String_t*, const RuntimeMethod*))CanvasUIMethodCall_1__ctor_m12C5E340F6FA46E90BED3201BB97BE4BEB0FB040_gshared)(__this, ___0_canvasImpl, ___1_methodName, ___2_callbackMethod, method);
}
// System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IPayResult>::set_Callback(Facebook.Unity.FacebookDelegate`1<T>)
inline void MethodCall_1_set_Callback_m2FE69B0E3B56B162D1F585BF08320F54F3EB73CB_inline (MethodCall_1_t222223E3C8CAEF78C55722F5474C0C2C66F0EA2B* __this, FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B* ___0_value, const RuntimeMethod* method)
{
	((  void (*) (MethodCall_1_t222223E3C8CAEF78C55722F5474C0C2C66F0EA2B*, FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B*, const RuntimeMethod*))MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline)(__this, ___0_value, method);
}
// System.Void Facebook.Unity.LoginResult::.ctor(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void LoginResult__ctor_m872B7ED7617120053FBB543555ECE68D51E04448 (LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.Canvas.CanvasFacebook/<>c::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__ctor_m9E708C2F94EE7BA080505D395F1D5B63F00D64FA (U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8* __this, const RuntimeMethod* method) ;
// Facebook.Unity.AccessToken Facebook.Unity.LoginResult::get_AccessToken()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346* LoginResult_get_AccessToken_m57A8C7CD5C7AE61792EEBBC7486FBD18C03EB7FE_inline (LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1* __this, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.AccessToken::set_CurrentAccessToken(Facebook.Unity.AccessToken)
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void AccessToken_set_CurrentAccessToken_m7C9C5DF205C036C288F21657E7891FF461BE9EB4_inline (AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346* ___0_value, const RuntimeMethod* method) ;
// System.Void System.Collections.Generic.List`1<System.String>::.ctor()
inline void List_1__ctor_mCA8DD57EAC70C2B5923DBB9D5A77CEAC22E7068E (List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* __this, const RuntimeMethod* method)
{
	((  void (*) (List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD*, const RuntimeMethod*))List_1__ctor_m7F078BB342729BDF11327FD89D7872265328F690_gshared)(__this, method);
}
// System.Boolean Facebook.Unity.Utilities::TryGetValue<System.Collections.Generic.IList`1<System.Object>>(System.Collections.Generic.IDictionary`2<System.String,System.Object>,System.String,T&)
inline bool Utilities_TryGetValue_TisIList_1_t6EE90D273EFCF5E7E4C37FAB712E70BB6F1B4BFF_mC232E1497C9BEC609103DB1B2BE3EDF69B141C16 (RuntimeObject* ___0_dictionary, String_t* ___1_key, RuntimeObject** ___2_value, const RuntimeMethod* method)
{
	return ((  bool (*) (RuntimeObject*, String_t*, RuntimeObject**, const RuntimeMethod*))Utilities_TryGetValue_TisRuntimeObject_mFCB0A986B3D5F7A7BC16BB7A06D3639579533C12_gshared)(___0_dictionary, ___1_key, ___2_value, method);
}
// System.Boolean Facebook.Unity.Utilities::TryGetValue<System.String>(System.Collections.Generic.IDictionary`2<System.String,System.Object>,System.String,T&)
inline bool Utilities_TryGetValue_TisString_t_m2341652CD88C1BF40208CFE6D13B0EF59437D8CC (RuntimeObject* ___0_dictionary, String_t* ___1_key, String_t** ___2_value, const RuntimeMethod* method)
{
	return ((  bool (*) (RuntimeObject*, String_t*, String_t**, const RuntimeMethod*))Utilities_TryGetValue_TisRuntimeObject_mFCB0A986B3D5F7A7BC16BB7A06D3639579533C12_gshared)(___0_dictionary, ___1_key, ___2_value, method);
}
// System.Boolean System.String::Equals(System.String,System.StringComparison)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool String_Equals_m7BDFC0B951005B9DC2BAED464AFE68FF7E9ACE5A (String_t* __this, String_t* ___0_value, int32_t ___1_comparisonType, const RuntimeMethod* method) ;
// System.String Facebook.Unity.Utilities::ToCommaSeparateList(System.Collections.Generic.IEnumerable`1<System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* Utilities_ToCommaSeparateList_mCB4781E5AB2247A25AC99812FC901A171EDD747B (RuntimeObject* ___0_list, const RuntimeMethod* method) ;
// Facebook.Unity.Canvas.ICanvasFacebookImplementation Facebook.Unity.Canvas.CanvasFacebookGameObject::get_CanvasFacebookImpl()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* CanvasFacebookGameObject_get_CanvasFacebookImpl_m820D790D560FE4C671A11206517112613C721FA0 (CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* __this, const RuntimeMethod* method) ;
// System.Void UnityEngine.GameObject::.ctor(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GameObject__ctor_m37D512B05D292F954792225E6C6EEE95293A9B88 (GameObject_t76FEDD663AB33C991A9C9A23129337651094216F* __this, String_t* ___0_name, const RuntimeMethod* method) ;
// T UnityEngine.GameObject::AddComponent<Facebook.Unity.Canvas.JsBridge>()
inline JsBridge_t338341DF8272C935803F827012749337260971DA* GameObject_AddComponent_TisJsBridge_t338341DF8272C935803F827012749337260971DA_mE386CD7945F07943362FF25193BDB880EAFE1AE3 (GameObject_t76FEDD663AB33C991A9C9A23129337651094216F* __this, const RuntimeMethod* method)
{
	return ((  JsBridge_t338341DF8272C935803F827012749337260971DA* (*) (GameObject_t76FEDD663AB33C991A9C9A23129337651094216F*, const RuntimeMethod*))GameObject_AddComponent_TisRuntimeObject_m69B93700FACCF372F5753371C6E8FB780800B824_gshared)(__this, method);
}
// UnityEngine.Transform UnityEngine.GameObject::get_transform()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* GameObject_get_transform_m0BC10ADFA1632166AE5544BDF9038A2650C2AE56 (GameObject_t76FEDD663AB33C991A9C9A23129337651094216F* __this, const RuntimeMethod* method) ;
// UnityEngine.GameObject UnityEngine.Component::get_gameObject()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR GameObject_t76FEDD663AB33C991A9C9A23129337651094216F* Component_get_gameObject_m57AEFBB14DB39EC476F740BA000E170355DE691B (Component_t39FBE53E5EFCF4409111FB22C15FF73717632EC3* __this, const RuntimeMethod* method) ;
// System.Void UnityEngine.Transform::set_parent(UnityEngine.Transform)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Transform_set_parent_m9BD5E563B539DD5BEC342736B03F97B38A243234 (Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* __this, Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* ___0_value, const RuntimeMethod* method) ;
// System.Void Facebook.Unity.FacebookGameObject::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void FacebookGameObject__ctor_mA095CF29FC6CD7D4C5712F356B75C0F845C954AF (FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B* __this, const RuntimeMethod* method) ;
// T Facebook.Unity.ComponentFactory::GetComponent<Facebook.Unity.Canvas.CanvasFacebookGameObject>(Facebook.Unity.ComponentFactory/IfNotExist)
inline CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* ComponentFactory_GetComponent_TisCanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46_mE51621647A97AC742D31A41B486013594290ED7B (int32_t ___0_ifNotExist, const RuntimeMethod* method)
{
	return ((  CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* (*) (int32_t, const RuntimeMethod*))ComponentFactory_GetComponent_TisRuntimeObject_m2E16AB838B228E32DF5CA1DBF301A0A99BF99E9C_gshared)(___0_ifNotExist, method);
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook__ctor_m84A06718D125F7DEC19BD186F5D318F495248EFE (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, const RuntimeMethod* method) ;
// System.Boolean System.String::op_Inequality(System.String,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool String_op_Inequality_m8C940F3CFC42866709D7CA931B3D77B4BE94BCB6 (String_t* ___0_a, String_t* ___1_b, const RuntimeMethod* method) ;
// System.Void UnityEngine.MonoBehaviour::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void MonoBehaviour__ctor_m592DB0105CA0BC97AA1C5F4AD27B12D68A3B7C1E (MonoBehaviour_t532A11E69716D348D8AA7F854AFCBFCB8AD17F71* __this, const RuntimeMethod* method) ;
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NativeDict__ctor_m3A0EA06026FAAD1574D34316CE425641C08AA531 (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		NativeDict_set_NumEntries_m57353835AD86ED502693E4BE8DCE1E3265BD6B52_inline(__this, 0, NULL);
		NativeDict_set_Keys_m30955DED0955CCE506D1FC315F2F534BF1450D7D_inline(__this, (StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248*)NULL, NULL);
		NativeDict_set_Values_m2D082203061E2C96CD3F728152BB43361E3CD860_inline(__this, (StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248*)NULL, NULL);
		return;
	}
}
// System.Int32 Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::get_NumEntries()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t NativeDict_get_NumEntries_mFB13D8178B1556ABF10D679F54B405819292FCDB (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, const RuntimeMethod* method) 
{
	{
		int32_t L_0 = __this->___U3CNumEntriesU3Ek__BackingField_0;
		return L_0;
	}
}
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::set_NumEntries(System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NativeDict_set_NumEntries_m57353835AD86ED502693E4BE8DCE1E3265BD6B52 (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, int32_t ___0_value, const RuntimeMethod* method) 
{
	{
		int32_t L_0 = ___0_value;
		__this->___U3CNumEntriesU3Ek__BackingField_0 = L_0;
		return;
	}
}
// System.String[] Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::get_Keys()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* NativeDict_get_Keys_mE731082312A3966531B454E2EE0228E0BA14A4B3 (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, const RuntimeMethod* method) 
{
	{
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_0 = __this->___U3CKeysU3Ek__BackingField_1;
		return L_0;
	}
}
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::set_Keys(System.String[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NativeDict_set_Keys_m30955DED0955CCE506D1FC315F2F534BF1450D7D (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___0_value, const RuntimeMethod* method) 
{
	{
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_0 = ___0_value;
		__this->___U3CKeysU3Ek__BackingField_1 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CKeysU3Ek__BackingField_1), (void*)L_0);
		return;
	}
}
// System.String[] Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::get_Values()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* NativeDict_get_Values_m201D111E57F4837102CF02561EDEDBD7FBBBA2C1 (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, const RuntimeMethod* method) 
{
	{
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_0 = __this->___U3CValuesU3Ek__BackingField_2;
		return L_0;
	}
}
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebook/NativeDict::set_Values(System.String[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void NativeDict_set_Values_m2D082203061E2C96CD3F728152BB43361E3CD860 (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___0_value, const RuntimeMethod* method) 
{
	{
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_0 = ___0_value;
		__this->___U3CValuesU3Ek__BackingField_2 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CValuesU3Ek__BackingField_2), (void*)L_0);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebookGameObject::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void IOSFacebookGameObject__ctor_m2C8F511F0B941CD1BF051275BCF53174396854AD (IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA* __this, const RuntimeMethod* method) 
{
	{
		MobileFacebookGameObject__ctor_m21F7B347AB7787EBE63D37862A1BB5F191E9B17D(__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Facebook.Unity.FacebookGameObject Facebook.Unity.Mobile.IOS.IOSFacebookLoader::get_FBGameObject()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B* IOSFacebookLoader_get_FBGameObject_mE7F3A444000BC99D3EC59046DB5AC217010299B2 (IOSFacebookLoader_tC4449925F5E12605B6DAC60A202C0EC3ECB7B242* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ComponentFactory_GetComponent_TisIOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA_m1537424BCE59DE26311F38D9FDC0B8F90AAEC688_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IOSFacebook_tC5DD1DAB3274516F7194A166C4C9D5C2900A99F4_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA* V_0 = NULL;
	{
		IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA* L_0;
		L_0 = ComponentFactory_GetComponent_TisIOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA_m1537424BCE59DE26311F38D9FDC0B8F90AAEC688(0, ComponentFactory_GetComponent_TisIOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA_m1537424BCE59DE26311F38D9FDC0B8F90AAEC688_RuntimeMethod_var);
		V_0 = L_0;
		IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA* L_1 = V_0;
		NullCheck(L_1);
		RuntimeObject* L_2;
		L_2 = FacebookGameObject_get_Facebook_m8F6DC9F80E732D237D7F858FB1FC5D448071D137_inline(L_1, NULL);
		if (L_2)
		{
			goto IL_001a;
		}
	}
	{
		IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA* L_3 = V_0;
		IOSFacebook_tC5DD1DAB3274516F7194A166C4C9D5C2900A99F4* L_4 = (IOSFacebook_tC5DD1DAB3274516F7194A166C4C9D5C2900A99F4*)il2cpp_codegen_object_new(IOSFacebook_tC5DD1DAB3274516F7194A166C4C9D5C2900A99F4_il2cpp_TypeInfo_var);
		NullCheck(L_4);
		IOSFacebook__ctor_m09F8C5E4063FAB2C5C2084B32F924630161331AB(L_4, NULL);
		NullCheck(L_3);
		FacebookGameObject_set_Facebook_mEDCEAC5301992E16A962C3993F25C151F6251F1C_inline(L_3, L_4, NULL);
	}

IL_001a:
	{
		IOSFacebookGameObject_t450135CB062F631777F4608FB602AC5202797ACA* L_5 = V_0;
		return L_5;
	}
}
// System.Void Facebook.Unity.Mobile.IOS.IOSFacebookLoader::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void IOSFacebookLoader__ctor_m11A9478D7F674F2FA8D10FF31469F5D65D65B89B (IOSFacebookLoader_tC4449925F5E12605B6DAC60A202C0EC3ECB7B242* __this, const RuntimeMethod* method) 
{
	{
		CompiledFacebookLoader__ctor_m0CEF02AE4B19FCB945491FD9DE8B4CAB0CB4F227(__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook__ctor_m4C9297CE471F88A30A971963689FAE8332984011 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0;
		L_0 = AndroidFacebook_GetAndroidWrapper_mFB879F02BAD046A4C58F0A4AFCADE56BB98673A9(NULL);
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_1 = (CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749*)il2cpp_codegen_object_new(CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		CallbackManager__ctor_mBDDC9E4FCC6A9A0CCDE01755DC232BFCA66D4BA8(L_1, NULL);
		AndroidFacebook__ctor_m9AE45755787625750D7BB161746CCF187F2D327B(__this, L_0, L_1, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::.ctor(Facebook.Unity.Mobile.Android.IAndroidWrapper,Facebook.Unity.CallbackManager)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook__ctor_m9AE45755787625750D7BB161746CCF187F2D327B (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, RuntimeObject* ___0_androidWrapper, CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* ___1_callbackManager, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&String_t_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_0 = ___1_callbackManager;
		MobileFacebook__ctor_m6A029489BABC6CB3915882605CC3F8E4EFD3DFD1(__this, L_0, NULL);
		String_t* L_1 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->___Empty_6;
		AndroidFacebook_set_KeyHash_m955883E7FF2093A8219358F222D292B3E556FED1_inline(__this, L_1, NULL);
		RuntimeObject* L_2 = ___0_androidWrapper;
		__this->___androidWrapper_5 = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___androidWrapper_5), (void*)L_2);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::set_KeyHash(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_set_KeyHash_m955883E7FF2093A8219358F222D292B3E556FED1 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_value, const RuntimeMethod* method) 
{
	{
		String_t* L_0 = ___0_value;
		__this->___U3CKeyHashU3Ek__BackingField_7 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CKeyHashU3Ek__BackingField_7), (void*)L_0);
		return;
	}
}
// System.Boolean Facebook.Unity.Mobile.Android.AndroidFacebook::get_LimitEventUsage()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool AndroidFacebook_get_LimitEventUsage_m7E545EC37F53B3B67CBF85DBEDA016134B592213 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, const RuntimeMethod* method) 
{
	{
		bool L_0 = __this->___limitEventUsage_4;
		return L_0;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::set_LimitEventUsage(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_set_LimitEventUsage_m889CE583C1A25F566D5957D85FA3C8E5F1A703FA (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, bool ___0_value, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralD520D5471D708F3D7ED6BA9DBE14564EB942076B);
		s_Il2CppMethodInitialized = true;
	}
	{
		bool L_0 = ___0_value;
		__this->___limitEventUsage_4 = L_0;
		String_t* L_1;
		L_1 = Boolean_ToString_m6646C8026B1DF381A1EE8CD13549175E9703CC63((&___0_value), NULL);
		AndroidFacebook_CallFB_mBFD340DDC8C5FD7AD5B9498EC31A03C371448889(__this, _stringLiteralD520D5471D708F3D7ED6BA9DBE14564EB942076B, L_1, NULL);
		return;
	}
}
// System.String Facebook.Unity.Mobile.Android.AndroidFacebook::get_SDKName()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* AndroidFacebook_get_SDKName_m45A7073C38377B225083ABF9EDD945C33740EF72 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralBCAB68EDA292B4050A6B8D0864BDB3D03423FBB3);
		s_Il2CppMethodInitialized = true;
	}
	{
		return _stringLiteralBCAB68EDA292B4050A6B8D0864BDB3D03423FBB3;
	}
}
// System.String Facebook.Unity.Mobile.Android.AndroidFacebook::get_SDKVersion()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* AndroidFacebook_get_SDKVersion_m2B6B6A54835EFE1C041F5585CE9F89B7619AF829 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidWrapper_CallStatic_TisString_t_m53CBBE3DF66F9F471C84A81702233C750461949A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral77D2C3EA46482F5FF2F649EFEDC038D051B91098);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___androidWrapper_5;
		NullCheck(L_0);
		String_t* L_1;
		L_1 = GenericInterfaceFuncInvoker1< String_t*, String_t* >::Invoke(IAndroidWrapper_CallStatic_TisString_t_m53CBBE3DF66F9F471C84A81702233C750461949A_RuntimeMethod_var, L_0, _stringLiteral77D2C3EA46482F5FF2F649EFEDC038D051B91098);
		return L_1;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::Init(System.String,Facebook.Unity.HideUnityDelegate,Facebook.Unity.InitDelegate)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_Init_m53036A5EFD47526D15A30AFCD4B0BB1E25F6318E (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_appId, HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE* ___1_hideUnityDelegate, InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F* ___2_onInitComplete, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidWrapper_CallStatic_TisString_t_m53CBBE3DF66F9F471C84A81702233C750461949A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral30F20681B7D1BDA035E4777AE6F8EF22F97224FC);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral3C74EE53B1AF65557F9BDF1EAF0C416BADC79DB9);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral897F348B17F4F3F52BB5A958E8F16003D3831485);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA50738BC6FDE22FAA8A8A82E2979A8FFA42C93A4);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		String_t* L_0;
		L_0 = Constants_get_UnitySDKUserAgentSuffixLegacy_mC9A4AA341CF075CD6CA1B6D37E63BCB56A40BB60(NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)0);
		String_t* L_2;
		L_2 = String_Format_m918500C1EFB475181349A79989BB79BB36102894(L_0, L_1, NULL);
		AndroidFacebook_CallFB_mBFD340DDC8C5FD7AD5B9498EC31A03C371448889(__this, _stringLiteral897F348B17F4F3F52BB5A958E8F16003D3831485, L_2, NULL);
		InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F* L_3 = ___2_onInitComplete;
		FacebookBase_Init_m20FB2F18FDA2C3A4FA3EAD6C734EEFEAA5CF5F6A_inline(__this, L_3, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_4 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_4);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_4, NULL);
		V_0 = L_4;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		String_t* L_6 = ___0_appId;
		NullCheck(L_5);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_5, _stringLiteral30F20681B7D1BDA035E4777AE6F8EF22F97224FC, L_6, NULL);
		JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26* L_7 = (JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26*)il2cpp_codegen_object_new(JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26_il2cpp_TypeInfo_var);
		NullCheck(L_7);
		JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06(L_7, __this, _stringLiteral3C74EE53B1AF65557F9BDF1EAF0C416BADC79DB9, JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_8 = V_0;
		NullCheck(L_7);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IResult>::Call(Facebook.Unity.MethodArguments) */, L_7, L_8);
		RuntimeObject* L_9 = __this->___androidWrapper_5;
		NullCheck(L_9);
		String_t* L_10;
		L_10 = GenericInterfaceFuncInvoker1< String_t*, String_t* >::Invoke(IAndroidWrapper_CallStatic_TisString_t_m53CBBE3DF66F9F471C84A81702233C750461949A_RuntimeMethod_var, L_9, _stringLiteralA50738BC6FDE22FAA8A8A82E2979A8FFA42C93A4);
		__this->___userID_6 = L_10;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___userID_6), (void*)L_10);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::LoginWithTrackingPreference(System.String,System.Collections.Generic.IEnumerable`1<System.String>,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ILoginResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_LoginWithTrackingPreference_mF026DB690A6AD6632EB986A1F26C6AB689754BBA (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_tracking, RuntimeObject* ___1_permissions, String_t* ___2_nonce, FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* ___3_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Debug_t8394C7EEAECA3689C2C9B9DE9C7166D73596276F_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1D692E10D206F441AFD73AB51AFABF2D76739E52);
		s_Il2CppMethodInitialized = true;
	}
	{
		il2cpp_codegen_runtime_class_init_inline(Debug_t8394C7EEAECA3689C2C9B9DE9C7166D73596276F_il2cpp_TypeInfo_var);
		bool L_0;
		L_0 = Debug_get_isDebugBuild_m9277C4A9591F7E1D8B76340B4CAE5EA33D63AF01(NULL);
		if (!L_0)
		{
			goto IL_0011;
		}
	}
	{
		il2cpp_codegen_runtime_class_init_inline(Debug_t8394C7EEAECA3689C2C9B9DE9C7166D73596276F_il2cpp_TypeInfo_var);
		Debug_Log_m87A9A3C761FF5C43ED8A53B16190A53D08F818BB(_stringLiteral1D692E10D206F441AFD73AB51AFABF2D76739E52, NULL);
	}

IL_0011:
	{
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::LogInWithReadPermissions(System.Collections.Generic.IEnumerable`1<System.String>,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ILoginResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_LogInWithReadPermissions_m4C99FF88B77AB3DB605AF8917E3A381A3ACFC8AD (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, RuntimeObject* ___0_permissions, FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mEDBCDC346A8A1302DA0A03038DF6159D0DA5FB9E_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m6AE07A293D1DB63C07A09D30AC74C42874098633_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral49DF6F7C16A7531B4C585988196CC8F4BAECBE73);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4D1BDBFCC51F4695254B3C53B7142111EC52EA17);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		RuntimeObject* L_2 = ___0_permissions;
		NullCheck(L_1);
		MethodArguments_AddCommaSeparatedList_m4E3E59B109499EC3D6810CBEE16437DB193E0C38(L_1, _stringLiteral4D1BDBFCC51F4695254B3C53B7142111EC52EA17, L_2, NULL);
		JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96* L_3 = (JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96*)il2cpp_codegen_object_new(JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96_il2cpp_TypeInfo_var);
		NullCheck(L_3);
		JavaMethodCall_1__ctor_mEDBCDC346A8A1302DA0A03038DF6159D0DA5FB9E(L_3, __this, _stringLiteral49DF6F7C16A7531B4C585988196CC8F4BAECBE73, JavaMethodCall_1__ctor_mEDBCDC346A8A1302DA0A03038DF6159D0DA5FB9E_RuntimeMethod_var);
		JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96* L_4 = L_3;
		FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* L_5 = ___1_callback;
		NullCheck(L_4);
		MethodCall_1_set_Callback_m6AE07A293D1DB63C07A09D30AC74C42874098633_inline(L_4, L_5, MethodCall_1_set_Callback_m6AE07A293D1DB63C07A09D30AC74C42874098633_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		NullCheck(L_4);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ILoginResult>::Call(Facebook.Unity.MethodArguments) */, L_4, L_6);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::LogInWithPublishPermissions(System.Collections.Generic.IEnumerable`1<System.String>,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ILoginResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_LogInWithPublishPermissions_mFB6B7D47B8365217835469A3AF4FE77D6BE35F44 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, RuntimeObject* ___0_permissions, FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mEDBCDC346A8A1302DA0A03038DF6159D0DA5FB9E_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m6AE07A293D1DB63C07A09D30AC74C42874098633_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4D1BDBFCC51F4695254B3C53B7142111EC52EA17);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA51D1F1AA9592A683F8519425630E22AA5F35FD1);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		RuntimeObject* L_2 = ___0_permissions;
		NullCheck(L_1);
		MethodArguments_AddCommaSeparatedList_m4E3E59B109499EC3D6810CBEE16437DB193E0C38(L_1, _stringLiteral4D1BDBFCC51F4695254B3C53B7142111EC52EA17, L_2, NULL);
		JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96* L_3 = (JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96*)il2cpp_codegen_object_new(JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96_il2cpp_TypeInfo_var);
		NullCheck(L_3);
		JavaMethodCall_1__ctor_mEDBCDC346A8A1302DA0A03038DF6159D0DA5FB9E(L_3, __this, _stringLiteralA51D1F1AA9592A683F8519425630E22AA5F35FD1, JavaMethodCall_1__ctor_mEDBCDC346A8A1302DA0A03038DF6159D0DA5FB9E_RuntimeMethod_var);
		JavaMethodCall_1_tB3022C37297D28CE6F5757195ADE8F9178F71C96* L_4 = L_3;
		FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* L_5 = ___1_callback;
		NullCheck(L_4);
		MethodCall_1_set_Callback_m6AE07A293D1DB63C07A09D30AC74C42874098633_inline(L_4, L_5, MethodCall_1_set_Callback_m6AE07A293D1DB63C07A09D30AC74C42874098633_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		NullCheck(L_4);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ILoginResult>::Call(Facebook.Unity.MethodArguments) */, L_4, L_6);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::LogOut()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_LogOut_mC14352C07A4BF2C75C48B459239E995E42FD3126 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral80BFDED53A798895F66F7586BB93FB843218DB76);
		s_Il2CppMethodInitialized = true;
	}
	{
		FacebookBase_LogOut_m2FFCD340816DE9D0B0AD32D5DDF89DFED7C62751(__this, NULL);
		JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26* L_0 = (JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26*)il2cpp_codegen_object_new(JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06(L_0, __this, _stringLiteral80BFDED53A798895F66F7586BB93FB843218DB76, JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06_RuntimeMethod_var);
		NullCheck(L_0);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IResult>::Call(Facebook.Unity.MethodArguments) */, L_0, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// Facebook.Unity.AuthenticationToken Facebook.Unity.Mobile.Android.AndroidFacebook::CurrentAuthenticationToken()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AuthenticationToken_t925F4C42BE7D08897D579F0A55D5682704237B8C* AndroidFacebook_CurrentAuthenticationToken_m6C213D8F6F7EAD81C6384589365B6811D97E5CC6 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, const RuntimeMethod* method) 
{
	{
		return (AuthenticationToken_t925F4C42BE7D08897D579F0A55D5682704237B8C*)NULL;
	}
}
// Facebook.Unity.Profile Facebook.Unity.Mobile.Android.AndroidFacebook::CurrentProfile()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7* AndroidFacebook_CurrentProfile_m0ACF33A89FC8F58FBFBCE521C0B76161AFFC1E26 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidWrapper_CallStatic_TisString_t_m53CBBE3DF66F9F471C84A81702233C750461949A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral03AF4AAE45F0FD9CE9D36A119A4A931D2A7620AD);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1DDC3BD3066DA80913E08F31F0BE455DC5ECF9A3);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral363C72B549019095B81245E1ADCD3F7DE027672D);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral51921D99887DD5ED233F87333EF648AE91A8BF7C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral678830C5836DCD590137DA23DA474CD589366649);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral742AC0EDA1CCFC3576DC3F77C0A7F4EB0F9923C4);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral81861CA7BE722F39376AE14F09BA19F73DB86EBF);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral84BD1476CFD81DB95A69E18C0BD3E1DE29BD872F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral85EA23745BAB27C1CF9C76837E55332314BA92E0);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral864CC40A200813B9284307874D1D3C8ACD06DE8C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral9D7EFF3063C8C498DC4376D8A7C77CBD3894B949);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralCA1511AE7356E5E0E5B6B5905112292E8DF67CB2);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralCE18B047107AA23D1AA9B2ED32D316148E02655F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF7F2EA94F25E42499DF1BBEA8E07B2BB10492332);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	RuntimeObject* V_1 = NULL;
	String_t* V_2 = NULL;
	String_t* V_3 = NULL;
	String_t* V_4 = NULL;
	String_t* V_5 = NULL;
	String_t* V_6 = NULL;
	String_t* V_7 = NULL;
	String_t* V_8 = NULL;
	String_t* V_9 = NULL;
	String_t* V_10 = NULL;
	String_t* V_11 = NULL;
	String_t* V_12 = NULL;
	UserAgeRange_t3074872BAC5CB213D6E7245C5C9407CA12B7321A* V_13 = NULL;
	FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* V_14 = NULL;
	FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* V_15 = NULL;
	Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7* V_16 = NULL;
	il2cpp::utils::ExceptionSupportStack<RuntimeObject*, 1> __active_exceptions;
	String_t* G_B3_0 = NULL;
	String_t* G_B3_1 = NULL;
	String_t* G_B3_2 = NULL;
	String_t* G_B3_3 = NULL;
	String_t* G_B3_4 = NULL;
	String_t* G_B3_5 = NULL;
	String_t* G_B3_6 = NULL;
	String_t* G_B3_7 = NULL;
	String_t* G_B2_0 = NULL;
	String_t* G_B2_1 = NULL;
	String_t* G_B2_2 = NULL;
	String_t* G_B2_3 = NULL;
	String_t* G_B2_4 = NULL;
	String_t* G_B2_5 = NULL;
	String_t* G_B2_6 = NULL;
	String_t* G_B2_7 = NULL;
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* G_B4_0 = NULL;
	String_t* G_B4_1 = NULL;
	String_t* G_B4_2 = NULL;
	String_t* G_B4_3 = NULL;
	String_t* G_B4_4 = NULL;
	String_t* G_B4_5 = NULL;
	String_t* G_B4_6 = NULL;
	String_t* G_B4_7 = NULL;
	String_t* G_B4_8 = NULL;
	{
		RuntimeObject* L_0 = __this->___androidWrapper_5;
		NullCheck(L_0);
		String_t* L_1;
		L_1 = GenericInterfaceFuncInvoker1< String_t*, String_t* >::Invoke(IAndroidWrapper_CallStatic_TisString_t_m53CBBE3DF66F9F471C84A81702233C750461949A_RuntimeMethod_var, L_0, _stringLiteral742AC0EDA1CCFC3576DC3F77C0A7F4EB0F9923C4);
		V_0 = L_1;
		String_t* L_2 = V_0;
		bool L_3;
		L_3 = String_IsNullOrEmpty_mEA9E3FB005AC28FE02E69FCF95A7B8456192B478(L_2, NULL);
		if (L_3)
		{
			goto IL_0124;
		}
	}
	try
	{// begin try (depth: 1)
		{
			String_t* L_4 = V_0;
			RuntimeObject* L_5;
			L_5 = Utilities_ParseStringDictionaryFromString_mC44A9B19967AC7A42BD9038BFB475AC8700A8AB5(L_4, NULL);
			V_1 = L_5;
			RuntimeObject* L_6 = V_1;
			NullCheck(L_6);
			bool L_7;
			L_7 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_6, _stringLiteralF7F2EA94F25E42499DF1BBEA8E07B2BB10492332, (&V_2));
			RuntimeObject* L_8 = V_1;
			NullCheck(L_8);
			bool L_9;
			L_9 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_8, _stringLiteralCA1511AE7356E5E0E5B6B5905112292E8DF67CB2, (&V_3));
			RuntimeObject* L_10 = V_1;
			NullCheck(L_10);
			bool L_11;
			L_11 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_10, _stringLiteral864CC40A200813B9284307874D1D3C8ACD06DE8C, (&V_4));
			RuntimeObject* L_12 = V_1;
			NullCheck(L_12);
			bool L_13;
			L_13 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_12, _stringLiteral678830C5836DCD590137DA23DA474CD589366649, (&V_5));
			RuntimeObject* L_14 = V_1;
			NullCheck(L_14);
			bool L_15;
			L_15 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_14, _stringLiteralCE18B047107AA23D1AA9B2ED32D316148E02655F, (&V_6));
			RuntimeObject* L_16 = V_1;
			NullCheck(L_16);
			bool L_17;
			L_17 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_16, _stringLiteral51921D99887DD5ED233F87333EF648AE91A8BF7C, (&V_7));
			RuntimeObject* L_18 = V_1;
			NullCheck(L_18);
			bool L_19;
			L_19 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_18, _stringLiteral84BD1476CFD81DB95A69E18C0BD3E1DE29BD872F, (&V_8));
			RuntimeObject* L_20 = V_1;
			NullCheck(L_20);
			bool L_21;
			L_21 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_20, _stringLiteral81861CA7BE722F39376AE14F09BA19F73DB86EBF, (&V_9));
			RuntimeObject* L_22 = V_1;
			NullCheck(L_22);
			bool L_23;
			L_23 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_22, _stringLiteral363C72B549019095B81245E1ADCD3F7DE027672D, (&V_10));
			RuntimeObject* L_24 = V_1;
			NullCheck(L_24);
			bool L_25;
			L_25 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_24, _stringLiteral9D7EFF3063C8C498DC4376D8A7C77CBD3894B949, (&V_11));
			RuntimeObject* L_26 = V_1;
			NullCheck(L_26);
			bool L_27;
			L_27 = InterfaceFuncInvoker2< bool, String_t*, String_t** >::Invoke(7 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.String>::TryGetValue(TKey,TValue&) */, IDictionary_2_t51DBA2F8AFDC8E5CC588729B12034B8C4D30B0AF_il2cpp_TypeInfo_var, L_26, _stringLiteral85EA23745BAB27C1CF9C76837E55332314BA92E0, (&V_12));
			RuntimeObject* L_28 = V_1;
			UserAgeRange_t3074872BAC5CB213D6E7245C5C9407CA12B7321A* L_29;
			L_29 = UserAgeRange_AgeRangeFromDictionary_mE6F627CE372837D202F09D5C8F08DCD8737F9809(L_28, NULL);
			V_13 = L_29;
			RuntimeObject* L_30 = V_1;
			FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* L_31;
			L_31 = FBLocation_FromDictionary_mD50DFBAB5217A454D51D465E834AE81A2DE6CDAD(_stringLiteral1DDC3BD3066DA80913E08F31F0BE455DC5ECF9A3, L_30, NULL);
			V_14 = L_31;
			RuntimeObject* L_32 = V_1;
			FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* L_33;
			L_33 = FBLocation_FromDictionary_mD50DFBAB5217A454D51D465E834AE81A2DE6CDAD(_stringLiteral03AF4AAE45F0FD9CE9D36A119A4A931D2A7620AD, L_32, NULL);
			V_15 = L_33;
			String_t* L_34 = __this->___userID_6;
			String_t* L_35 = V_3;
			String_t* L_36 = V_4;
			String_t* L_37 = V_5;
			String_t* L_38 = V_6;
			String_t* L_39 = V_7;
			String_t* L_40 = V_8;
			String_t* L_41 = V_9;
			String_t* L_42 = V_10;
			G_B2_0 = L_41;
			G_B2_1 = L_40;
			G_B2_2 = L_39;
			G_B2_3 = L_38;
			G_B2_4 = L_37;
			G_B2_5 = L_36;
			G_B2_6 = L_35;
			G_B2_7 = L_34;
			if (L_42)
			{
				G_B3_0 = L_41;
				G_B3_1 = L_40;
				G_B3_2 = L_39;
				G_B3_3 = L_38;
				G_B3_4 = L_37;
				G_B3_5 = L_36;
				G_B3_6 = L_35;
				G_B3_7 = L_34;
				goto IL_00f9_1;
			}
		}
		{
			G_B4_0 = ((StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248*)(NULL));
			G_B4_1 = G_B2_0;
			G_B4_2 = G_B2_1;
			G_B4_3 = G_B2_2;
			G_B4_4 = G_B2_3;
			G_B4_5 = G_B2_4;
			G_B4_6 = G_B2_5;
			G_B4_7 = G_B2_6;
			G_B4_8 = G_B2_7;
			goto IL_010b_1;
		}

IL_00f9_1:
		{
			String_t* L_43 = V_10;
			CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB* L_44 = (CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB*)(CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB*)SZArrayNew(CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB_il2cpp_TypeInfo_var, (uint32_t)1);
			CharU5BU5D_t799905CF001DD5F13F7DBB310181FC4D8B7D0AAB* L_45 = L_44;
			NullCheck(L_45);
			(L_45)->SetAt(static_cast<il2cpp_array_size_t>(0), (Il2CppChar)((int32_t)44));
			NullCheck(L_43);
			StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_46;
			L_46 = String_Split_m101D35FEC86371D2BB4E3480F6F896880093B2E9(L_43, L_45, NULL);
			G_B4_0 = L_46;
			G_B4_1 = G_B3_0;
			G_B4_2 = G_B3_1;
			G_B4_3 = G_B3_2;
			G_B4_4 = G_B3_3;
			G_B4_5 = G_B3_4;
			G_B4_6 = G_B3_5;
			G_B4_7 = G_B3_6;
			G_B4_8 = G_B3_7;
		}

IL_010b_1:
		{
			String_t* L_47 = V_11;
			UserAgeRange_t3074872BAC5CB213D6E7245C5C9407CA12B7321A* L_48 = V_13;
			FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* L_49 = V_14;
			FBLocation_tECD476C60A6E69B23C274757F0CCA02639C67236* L_50 = V_15;
			String_t* L_51 = V_12;
			Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7* L_52 = (Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7*)il2cpp_codegen_object_new(Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7_il2cpp_TypeInfo_var);
			NullCheck(L_52);
			Profile__ctor_mDDFECEFF520F5C549B21BF3AB352D47D2F7B999C(L_52, G_B4_8, G_B4_7, G_B4_6, G_B4_5, G_B4_4, G_B4_3, G_B4_2, G_B4_1, G_B4_0, L_47, L_48, L_49, L_50, L_51, NULL);
			V_16 = L_52;
			goto IL_0126;
		}
	}// end try (depth: 1)
	catch(Il2CppExceptionWrapper& e)
	{
		if(il2cpp_codegen_class_is_assignable_from (((RuntimeClass*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&Exception_t_il2cpp_TypeInfo_var)), il2cpp_codegen_object_class(e.ex)))
		{
			IL2CPP_PUSH_ACTIVE_EXCEPTION(e.ex);
			goto CATCH_011e;
		}
		throw e;
	}

CATCH_011e:
	{// begin catch(System.Exception)
		V_16 = (Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7*)NULL;
		IL2CPP_POP_ACTIVE_EXCEPTION();
		goto IL_0126;
	}// end catch (depth: 1)

IL_0124:
	{
		return (Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7*)NULL;
	}

IL_0126:
	{
		Profile_t80D50211DC9ED35561A8B96A45C9C79AF07E5CF7* L_53 = V_16;
		return L_53;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::OnLoginStatusRetrieved(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_OnLoginStatusRetrieved_m65A558404816C7726AD1B59A65F8E35BD00A5F78 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7* V_0 = NULL;
	{
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_0 = ___0_resultContainer;
		LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7* L_1 = (LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7*)il2cpp_codegen_object_new(LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		LoginStatusResult__ctor_m9A4F408E6505B85F672717A3A4FB3F2221A645A4(L_1, L_0, NULL);
		V_0 = L_1;
		LoginStatusResult_tA8B88EE880D224D1D64788133A0BDB2AE7BC6CB7* L_2 = V_0;
		VirtualActionInvoker1< LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1* >::Invoke(49 /* System.Void Facebook.Unity.FacebookBase::OnAuthResponse(Facebook.Unity.LoginResult) */, __this, L_2);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::AppRequest(System.String,System.Nullable`1<Facebook.Unity.OGActionType>,System.String,System.Collections.Generic.IEnumerable`1<System.String>,System.Collections.Generic.IEnumerable`1<System.Object>,System.Collections.Generic.IEnumerable`1<System.String>,System.Nullable`1<System.Int32>,System.String,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppRequestResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_AppRequest_m5D32582F8592790E82693AEC169B97098D8B5586 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_message, Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73 ___1_actionType, String_t* ___2_objectId, RuntimeObject* ___3_to, RuntimeObject* ___4_filters, RuntimeObject* ___5_excludeIds, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___6_maxRecipients, String_t* ___7_data, String_t* ___8_title, FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2* ___9_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Enumerable_Any_TisRuntimeObject_m67CFBD544CF1D1C0C7E7457FDBDB81649DE26847_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Enumerable_First_TisRuntimeObject_mEFECF1B8C3201589C5AF34176DCBF8DD926642D6_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mF266633C897B9D87553E0CB9CF81EAE049A9A423_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_AddNullablePrimitive_TisOGActionType_t779FA0C877B3CD5177F27123B2E9CB4CEB14EC2E_mF55BAB58959DD27895825BFF8DE9DCEEAC13DED7_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m097F0A862D0980C829D48D394888D0370C9EFB85_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&String_t_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral415902D5F52543D5ED4C8B796FB49950BFA51EF0);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4BDCC8C1F6304193EA13F4AFB26677B5B8AF161A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5E9D981D162913B37E33D18FF771488A4A34346E);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA4419EF51FB63A77978E414E01AC1C9DCF20AA99);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralB7D525AABAD0632A9EC0EE33A7514F6C4892582B);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC7B4D926EF9532A71B25AEC040A33D52C926425F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralCB1BF9C3818A300118617C45409E803B828F1E9A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralD559C6D97E819D8E4EF7ACDC34C4E8D3DD314964);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	String_t* V_1 = NULL;
	{
		String_t* L_0 = ___0_message;
		Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73 L_1 = ___1_actionType;
		String_t* L_2 = ___2_objectId;
		RuntimeObject* L_3 = ___3_to;
		RuntimeObject* L_4 = ___4_filters;
		RuntimeObject* L_5 = ___5_excludeIds;
		Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 L_6 = ___6_maxRecipients;
		String_t* L_7 = ___7_data;
		String_t* L_8 = ___8_title;
		FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2* L_9 = ___9_callback;
		FacebookBase_ValidateAppRequestArgs_m90BA81706354BF100FBBCEDAD59FA567395266D8(__this, L_0, L_1, L_2, L_3, L_4, L_5, L_6, L_7, L_8, L_9, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_10 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_10);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_10, NULL);
		V_0 = L_10;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_11 = V_0;
		String_t* L_12 = ___0_message;
		NullCheck(L_11);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_11, _stringLiteralD559C6D97E819D8E4EF7ACDC34C4E8D3DD314964, L_12, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_13 = V_0;
		Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73 L_14 = ___1_actionType;
		NullCheck(L_13);
		MethodArguments_AddNullablePrimitive_TisOGActionType_t779FA0C877B3CD5177F27123B2E9CB4CEB14EC2E_mF55BAB58959DD27895825BFF8DE9DCEEAC13DED7(L_13, _stringLiteral415902D5F52543D5ED4C8B796FB49950BFA51EF0, L_14, MethodArguments_AddNullablePrimitive_TisOGActionType_t779FA0C877B3CD5177F27123B2E9CB4CEB14EC2E_mF55BAB58959DD27895825BFF8DE9DCEEAC13DED7_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_15 = V_0;
		String_t* L_16 = ___2_objectId;
		NullCheck(L_15);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_15, _stringLiteral5E9D981D162913B37E33D18FF771488A4A34346E, L_16, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_17 = V_0;
		RuntimeObject* L_18 = ___3_to;
		NullCheck(L_17);
		MethodArguments_AddCommaSeparatedList_m4E3E59B109499EC3D6810CBEE16437DB193E0C38(L_17, _stringLiteralA4419EF51FB63A77978E414E01AC1C9DCF20AA99, L_18, NULL);
		RuntimeObject* L_19 = ___4_filters;
		if (!L_19)
		{
			goto IL_0077;
		}
	}
	{
		RuntimeObject* L_20 = ___4_filters;
		bool L_21;
		L_21 = Enumerable_Any_TisRuntimeObject_m67CFBD544CF1D1C0C7E7457FDBDB81649DE26847(L_20, Enumerable_Any_TisRuntimeObject_m67CFBD544CF1D1C0C7E7457FDBDB81649DE26847_RuntimeMethod_var);
		if (!L_21)
		{
			goto IL_0077;
		}
	}
	{
		RuntimeObject* L_22 = ___4_filters;
		RuntimeObject* L_23;
		L_23 = Enumerable_First_TisRuntimeObject_mEFECF1B8C3201589C5AF34176DCBF8DD926642D6(L_22, Enumerable_First_TisRuntimeObject_mEFECF1B8C3201589C5AF34176DCBF8DD926642D6_RuntimeMethod_var);
		V_1 = ((String_t*)IsInstSealed((RuntimeObject*)L_23, String_t_il2cpp_TypeInfo_var));
		String_t* L_24 = V_1;
		if (!L_24)
		{
			goto IL_0077;
		}
	}
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_25 = V_0;
		String_t* L_26 = V_1;
		NullCheck(L_25);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_25, _stringLiteral4BDCC8C1F6304193EA13F4AFB26677B5B8AF161A, L_26, NULL);
	}

IL_0077:
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_27 = V_0;
		Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 L_28 = ___6_maxRecipients;
		NullCheck(L_27);
		MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A(L_27, _stringLiteralCB1BF9C3818A300118617C45409E803B828F1E9A, L_28, MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_29 = V_0;
		String_t* L_30 = ___7_data;
		NullCheck(L_29);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_29, _stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469, L_30, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_31 = V_0;
		String_t* L_32 = ___8_title;
		NullCheck(L_31);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_31, _stringLiteralC7B4D926EF9532A71B25AEC040A33D52C926425F, L_32, NULL);
		JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1* L_33 = (JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1*)il2cpp_codegen_object_new(JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1_il2cpp_TypeInfo_var);
		NullCheck(L_33);
		JavaMethodCall_1__ctor_mF266633C897B9D87553E0CB9CF81EAE049A9A423(L_33, __this, _stringLiteralB7D525AABAD0632A9EC0EE33A7514F6C4892582B, JavaMethodCall_1__ctor_mF266633C897B9D87553E0CB9CF81EAE049A9A423_RuntimeMethod_var);
		JavaMethodCall_1_t9C14EBFE925776CBC47C7D7123E7D932D2D0B3A1* L_34 = L_33;
		FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2* L_35 = ___9_callback;
		NullCheck(L_34);
		MethodCall_1_set_Callback_m097F0A862D0980C829D48D394888D0370C9EFB85_inline(L_34, L_35, MethodCall_1_set_Callback_m097F0A862D0980C829D48D394888D0370C9EFB85_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_36 = V_0;
		NullCheck(L_34);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IAppRequestResult>::Call(Facebook.Unity.MethodArguments) */, L_34, L_36);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::ShareLink(System.Uri,System.String,System.String,System.Uri,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IShareResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_ShareLink_mC90BE9560964BE19B57A8B629095500C509F5669 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___0_contentURL, String_t* ___1_contentTitle, String_t* ___2_contentDescription, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___3_photoURL, FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* ___4_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mCD8FA794A4409286C7AFB41CA358BD8620A1D230_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2400E7BE5B6DFE8885596899EABA1AE106FE5ED8);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral30ADD6F92E008EF7749F86EFEC1366D210E74DF1);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral3CCA51B7FCAD5C35FAA203055F3EE4C112A51BDA);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5B0C5D37C82B62629FC978A74FD935409F9DCD8A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralBCFD3E65D6CD9EF40881E41AFBC564269AC00026);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_2 = ___0_contentURL;
		NullCheck(L_1);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_1, _stringLiteral5B0C5D37C82B62629FC978A74FD935409F9DCD8A, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		String_t* L_4 = ___1_contentTitle;
		NullCheck(L_3);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_3, _stringLiteralBCFD3E65D6CD9EF40881E41AFBC564269AC00026, L_4, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		String_t* L_6 = ___2_contentDescription;
		NullCheck(L_5);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_5, _stringLiteral3CCA51B7FCAD5C35FAA203055F3EE4C112A51BDA, L_6, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_7 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_8 = ___3_photoURL;
		NullCheck(L_7);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_7, _stringLiteral30ADD6F92E008EF7749F86EFEC1366D210E74DF1, L_8, NULL);
		JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3* L_9 = (JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3*)il2cpp_codegen_object_new(JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3_il2cpp_TypeInfo_var);
		NullCheck(L_9);
		JavaMethodCall_1__ctor_mCD8FA794A4409286C7AFB41CA358BD8620A1D230(L_9, __this, _stringLiteral2400E7BE5B6DFE8885596899EABA1AE106FE5ED8, JavaMethodCall_1__ctor_mCD8FA794A4409286C7AFB41CA358BD8620A1D230_RuntimeMethod_var);
		JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3* L_10 = L_9;
		FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* L_11 = ___4_callback;
		NullCheck(L_10);
		MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_inline(L_10, L_11, MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_12 = V_0;
		NullCheck(L_10);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IShareResult>::Call(Facebook.Unity.MethodArguments) */, L_10, L_12);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::FeedShare(System.String,System.Uri,System.String,System.String,System.String,System.Uri,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IShareResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_FeedShare_mF664D633B15A786909B71DEF1371B858EBB19478 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_toId, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___1_link, String_t* ___2_linkName, String_t* ___3_linkCaption, String_t* ___4_linkDescription, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___5_picture, String_t* ___6_mediaSource, FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* ___7_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mCD8FA794A4409286C7AFB41CA358BD8620A1D230_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral02E19B255FDA406167DD7B953DDD571E02DEE7DD);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral18F572556D6BF6632E36F7AEF2204E0BB1DCD4D3);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5E89DFB28C21F05C783FF6C52C1E49EC5D97B20C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA65DD0DC43941EFA9FB37C63C1878CDEBE84FA20);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralAEBD978DBA6DE5C36DCBB6A466CF1183121BCEA8);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralB521C0BA23951421DC8BB58874348C6E1EEC2E77);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralB9E4F905776B9F5D49046F2E19FAF29798DB9E76);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralCC7BA483E7733CE41D9E3F9422E266682C65E9B2);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_toId;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralCC7BA483E7733CE41D9E3F9422E266682C65E9B2, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_4 = ___1_link;
		NullCheck(L_3);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_3, _stringLiteral18F572556D6BF6632E36F7AEF2204E0BB1DCD4D3, L_4, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		String_t* L_6 = ___2_linkName;
		NullCheck(L_5);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_5, _stringLiteralB521C0BA23951421DC8BB58874348C6E1EEC2E77, L_6, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_7 = V_0;
		String_t* L_8 = ___3_linkCaption;
		NullCheck(L_7);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_7, _stringLiteralB9E4F905776B9F5D49046F2E19FAF29798DB9E76, L_8, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_9 = V_0;
		String_t* L_10 = ___4_linkDescription;
		NullCheck(L_9);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_9, _stringLiteral02E19B255FDA406167DD7B953DDD571E02DEE7DD, L_10, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_11 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_12 = ___5_picture;
		NullCheck(L_11);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_11, _stringLiteral5E89DFB28C21F05C783FF6C52C1E49EC5D97B20C, L_12, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_13 = V_0;
		String_t* L_14 = ___6_mediaSource;
		NullCheck(L_13);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_13, _stringLiteralA65DD0DC43941EFA9FB37C63C1878CDEBE84FA20, L_14, NULL);
		JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3* L_15 = (JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3*)il2cpp_codegen_object_new(JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3_il2cpp_TypeInfo_var);
		NullCheck(L_15);
		JavaMethodCall_1__ctor_mCD8FA794A4409286C7AFB41CA358BD8620A1D230(L_15, __this, _stringLiteralAEBD978DBA6DE5C36DCBB6A466CF1183121BCEA8, JavaMethodCall_1__ctor_mCD8FA794A4409286C7AFB41CA358BD8620A1D230_RuntimeMethod_var);
		JavaMethodCall_1_tDD5671FAED463555B3E83881FE368D0A76678BB3* L_16 = L_15;
		FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* L_17 = ___7_callback;
		NullCheck(L_16);
		MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_inline(L_16, L_17, MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_18 = V_0;
		NullCheck(L_16);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IShareResult>::Call(Facebook.Unity.MethodArguments) */, L_16, L_18);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::GetAppLink(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppLinkResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_GetAppLink_mDF8BAFD77BAC492622874E85C779B973D39D30D9 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mC2486364FEAA45691A130BAFFF533BC1384E5BEF_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m00241290CE194EF41B420201C9BC57834F005704_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1A0B9E6C938C3916F9D9E2D5124BD4B98AFF0556);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B* L_0 = (JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B*)il2cpp_codegen_object_new(JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_mC2486364FEAA45691A130BAFFF533BC1384E5BEF(L_0, __this, _stringLiteral1A0B9E6C938C3916F9D9E2D5124BD4B98AFF0556, JavaMethodCall_1__ctor_mC2486364FEAA45691A130BAFFF533BC1384E5BEF_RuntimeMethod_var);
		JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B* L_1 = L_0;
		FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m00241290CE194EF41B420201C9BC57834F005704_inline(L_1, L_2, MethodCall_1_set_Callback_m00241290CE194EF41B420201C9BC57834F005704_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IAppLinkResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::AppEventsLogEvent(System.String,System.Nullable`1<System.Single>,System.Collections.Generic.Dictionary`2<System.String,System.Object>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_AppEventsLogEvent_mC6CE59C57DD5F9668F4D96126C2D2A6651A72112 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_logEvent, Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75 ___1_valueToSum, Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* ___2_parameters, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Nullable_1_GetValueOrDefault_m068A148705ED1E215A5E85D18BA6852B192DA419_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Nullable_1_get_HasValue_mC149B1C717AF506BBE8932F2C1DC86C378D17EA8_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral33846201AA643F30E3A9FE0E17D65FD9F27C98A7);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral8032FA5FDE1CC0823EB09A003B765A5D49AB566C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC611A012636D51B5EBBC7ADEBD3C8631EA8DAF13);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC972CCEF9725C97FA81EE0784238DBD804D49222);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	float V_1 = 0.0f;
	String_t* G_B2_0 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B2_1 = NULL;
	String_t* G_B1_0 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B1_1 = NULL;
	String_t* G_B3_0 = NULL;
	String_t* G_B3_1 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B3_2 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_logEvent;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralC972CCEF9725C97FA81EE0784238DBD804D49222, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		bool L_4;
		L_4 = Nullable_1_get_HasValue_mC149B1C717AF506BBE8932F2C1DC86C378D17EA8_inline((&___1_valueToSum), Nullable_1_get_HasValue_mC149B1C717AF506BBE8932F2C1DC86C378D17EA8_RuntimeMethod_var);
		G_B1_0 = _stringLiteral33846201AA643F30E3A9FE0E17D65FD9F27C98A7;
		G_B1_1 = L_3;
		if (L_4)
		{
			G_B2_0 = _stringLiteral33846201AA643F30E3A9FE0E17D65FD9F27C98A7;
			G_B2_1 = L_3;
			goto IL_0024;
		}
	}
	{
		G_B3_0 = ((String_t*)(NULL));
		G_B3_1 = G_B1_0;
		G_B3_2 = G_B1_1;
		goto IL_0038;
	}

IL_0024:
	{
		float L_5;
		L_5 = Nullable_1_GetValueOrDefault_m068A148705ED1E215A5E85D18BA6852B192DA419_inline((&___1_valueToSum), Nullable_1_GetValueOrDefault_m068A148705ED1E215A5E85D18BA6852B192DA419_RuntimeMethod_var);
		V_1 = L_5;
		il2cpp_codegen_runtime_class_init_inline(CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* L_6;
		L_6 = CultureInfo_get_InvariantCulture_mD1E96DC845E34B10F78CB744B0CB5D7D63CEB1E6(NULL);
		String_t* L_7;
		L_7 = Single_ToString_m534852BD7949AA972435783D7B96D0FFB09F6D6A((&V_1), L_6, NULL);
		G_B3_0 = L_7;
		G_B3_1 = G_B2_0;
		G_B3_2 = G_B2_1;
	}

IL_0038:
	{
		NullCheck(G_B3_2);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(G_B3_2, G_B3_1, G_B3_0, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_8 = V_0;
		Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* L_9 = ___2_parameters;
		NullCheck(L_8);
		MethodArguments_AddDictionary_m1633D0C3C4E5262F1F846414317BEED6C32EB75C(L_8, _stringLiteralC611A012636D51B5EBBC7ADEBD3C8631EA8DAF13, L_9, NULL);
		JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26* L_10 = (JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26*)il2cpp_codegen_object_new(JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26_il2cpp_TypeInfo_var);
		NullCheck(L_10);
		JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06(L_10, __this, _stringLiteral8032FA5FDE1CC0823EB09A003B765A5D49AB566C, JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_11 = V_0;
		NullCheck(L_10);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IResult>::Call(Facebook.Unity.MethodArguments) */, L_10, L_11);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::AppEventsLogPurchase(System.Single,System.String,System.Collections.Generic.Dictionary`2<System.String,System.Object>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_AppEventsLogPurchase_m612651B65F87BD6D0CCFF3F0C83E162B8F242E1F (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, float ___0_logPurchase, String_t* ___1_currency, Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* ___2_parameters, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral8032FA5FDE1CC0823EB09A003B765A5D49AB566C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral9FFC6FBB48B64BB14E755F227D08648099006DA9);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralAB3E708924BFB9D6B641A4B9F82FE5FE57F307B6);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC611A012636D51B5EBBC7ADEBD3C8631EA8DAF13);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		il2cpp_codegen_runtime_class_init_inline(CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* L_2;
		L_2 = CultureInfo_get_InvariantCulture_mD1E96DC845E34B10F78CB744B0CB5D7D63CEB1E6(NULL);
		String_t* L_3;
		L_3 = Single_ToString_m534852BD7949AA972435783D7B96D0FFB09F6D6A((&___0_logPurchase), L_2, NULL);
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteral9FFC6FBB48B64BB14E755F227D08648099006DA9, L_3, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_4 = V_0;
		String_t* L_5 = ___1_currency;
		NullCheck(L_4);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_4, _stringLiteralAB3E708924BFB9D6B641A4B9F82FE5FE57F307B6, L_5, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* L_7 = ___2_parameters;
		NullCheck(L_6);
		MethodArguments_AddDictionary_m1633D0C3C4E5262F1F846414317BEED6C32EB75C(L_6, _stringLiteralC611A012636D51B5EBBC7ADEBD3C8631EA8DAF13, L_7, NULL);
		JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26* L_8 = (JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26*)il2cpp_codegen_object_new(JavaMethodCall_1_t43958950763FB140FD57572A856E795427C35D26_il2cpp_TypeInfo_var);
		NullCheck(L_8);
		JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06(L_8, __this, _stringLiteral8032FA5FDE1CC0823EB09A003B765A5D49AB566C, JavaMethodCall_1__ctor_m492D088EE8C32A4B0FE22FEF68BD075590029F06_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_9 = V_0;
		NullCheck(L_8);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IResult>::Call(Facebook.Unity.MethodArguments) */, L_8, L_9);
		return;
	}
}
// System.Boolean Facebook.Unity.Mobile.Android.AndroidFacebook::IsImplicitPurchaseLoggingEnabled()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool AndroidFacebook_IsImplicitPurchaseLoggingEnabled_m423036B8A916964D878DC02F4F3BD6370B31CA92 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidWrapper_CallStatic_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_m853C4A273DD838FD248C6D7A55AFEAE879A39499_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral549765DD842AF67AD155DB8B5FBFA4737CE548FC);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___androidWrapper_5;
		NullCheck(L_0);
		bool L_1;
		L_1 = GenericInterfaceFuncInvoker1< bool, String_t* >::Invoke(IAndroidWrapper_CallStatic_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_m853C4A273DD838FD248C6D7A55AFEAE879A39499_RuntimeMethod_var, L_0, _stringLiteral549765DD842AF67AD155DB8B5FBFA4737CE548FC);
		return L_1;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::ActivateApp(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_ActivateApp_mCFC9C6B304E050FEF0408E1FC0BDD262EFE96CCB (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_appId, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC3843AE7EC90FEDFFEE3510C1C5E1D6356927416);
		s_Il2CppMethodInitialized = true;
	}
	{
		AndroidFacebook_CallFB_mBFD340DDC8C5FD7AD5B9498EC31A03C371448889(__this, _stringLiteralC3843AE7EC90FEDFFEE3510C1C5E1D6356927416, (String_t*)NULL, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::FetchDeferredAppLink(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppLinkResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_FetchDeferredAppLink_m3A10755959CED3ED891E70BDA8F3AF00F1B3A28A (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mC2486364FEAA45691A130BAFFF533BC1384E5BEF_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m00241290CE194EF41B420201C9BC57834F005704_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral852638DF9819B10101A7347933AC592E1A7AF86E);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B* L_1 = (JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B*)il2cpp_codegen_object_new(JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		JavaMethodCall_1__ctor_mC2486364FEAA45691A130BAFFF533BC1384E5BEF(L_1, __this, _stringLiteral852638DF9819B10101A7347933AC592E1A7AF86E, JavaMethodCall_1__ctor_mC2486364FEAA45691A130BAFFF533BC1384E5BEF_RuntimeMethod_var);
		JavaMethodCall_1_t5510884655C8B828ADBE6BC206A2C751994BCF4B* L_2 = L_1;
		FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D* L_3 = ___0_callback;
		NullCheck(L_2);
		MethodCall_1_set_Callback_m00241290CE194EF41B420201C9BC57834F005704_inline(L_2, L_3, MethodCall_1_set_Callback_m00241290CE194EF41B420201C9BC57834F005704_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_4 = V_0;
		NullCheck(L_2);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IAppLinkResult>::Call(Facebook.Unity.MethodArguments) */, L_2, L_4);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::RefreshCurrentAccessToken(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAccessTokenRefreshResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_RefreshCurrentAccessToken_m3308164AD5446470F3D2E0B2EB4306890A483187 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_t0A787A64D3187E98A865A198DDF444C008C79F35* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m07934BE9727E716B67132D5648DCC2FD307D5281_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m4EB4144CB0D6BAAD051358C0C21EA8D28E43A4DB_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralFF5C69F4E12AC1F06314CC73AC583302EE52390F);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F* L_0 = (JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F*)il2cpp_codegen_object_new(JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_m07934BE9727E716B67132D5648DCC2FD307D5281(L_0, __this, _stringLiteralFF5C69F4E12AC1F06314CC73AC583302EE52390F, JavaMethodCall_1__ctor_m07934BE9727E716B67132D5648DCC2FD307D5281_RuntimeMethod_var);
		JavaMethodCall_1_t8BF0FEE476F5DDF4A95EA215EE3155F0E08E068F* L_1 = L_0;
		FacebookDelegate_1_t0A787A64D3187E98A865A198DDF444C008C79F35* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m4EB4144CB0D6BAAD051358C0C21EA8D28E43A4DB_inline(L_1, L_2, MethodCall_1_set_Callback_m4EB4144CB0D6BAAD051358C0C21EA8D28E43A4DB_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IAccessTokenRefreshResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::OpenFriendFinderDialog(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IGamingServicesFriendFinderResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_OpenFriendFinderDialog_m41FB887EA6BB529507AB67D77E368530800AB16A (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_t55619BCDC758CB1C82277E6D8BA44ACA47C03DE2* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m9EDA60F8B74B5DE2F1D9781C1D09AE56F5F490FA_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m7B3DF88D41AC203597412EF880B1442EA25112B4_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral94107132E374EC913B72B94E192C66E8B0699CDA);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524* L_0 = (JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524*)il2cpp_codegen_object_new(JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_m9EDA60F8B74B5DE2F1D9781C1D09AE56F5F490FA(L_0, __this, _stringLiteral94107132E374EC913B72B94E192C66E8B0699CDA, JavaMethodCall_1__ctor_m9EDA60F8B74B5DE2F1D9781C1D09AE56F5F490FA_RuntimeMethod_var);
		JavaMethodCall_1_tEE28C3511E2849BB7DF6F9A220FBBF4E2C592524* L_1 = L_0;
		FacebookDelegate_1_t55619BCDC758CB1C82277E6D8BA44ACA47C03DE2* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m7B3DF88D41AC203597412EF880B1442EA25112B4_inline(L_1, L_2, MethodCall_1_set_Callback_m7B3DF88D41AC203597412EF880B1442EA25112B4_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IGamingServicesFriendFinderResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::UploadImageToMediaLibrary(System.String,System.Uri,System.Boolean,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IMediaUploadResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_UploadImageToMediaLibrary_m087EAA451D15BA1447884972213B0B2297D74B3A (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_caption, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___1_imageUri, bool ___2_shouldLaunchMediaDialog, FacebookDelegate_1_t7CA00F6A27B3FE85139590EFEA03B7C7C0D4A66D* ___3_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m23975F7FD0FE3A43155D75E53A95085F6504F9CD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m5ACC9D63CAC86405A106A94B35C11B8641FA2088_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral28CD321C015FD8117CD1B202457F438D5669BE3C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5B63100E80DAFEE5BA4AF4EDCCB7370ED6550264);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralCA6526ECED6720495FFEC32D4D606B18F2C3A081);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE4F9B45B1B50EBC9511B1FDA35C85D4AD027C583);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_caption;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteral5B63100E80DAFEE5BA4AF4EDCCB7370ED6550264, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_4 = ___1_imageUri;
		NullCheck(L_3);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_3, _stringLiteralE4F9B45B1B50EBC9511B1FDA35C85D4AD027C583, L_4, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		String_t* L_6;
		L_6 = Boolean_ToString_m6646C8026B1DF381A1EE8CD13549175E9703CC63((&___2_shouldLaunchMediaDialog), NULL);
		NullCheck(L_5);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_5, _stringLiteralCA6526ECED6720495FFEC32D4D606B18F2C3A081, L_6, NULL);
		JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA* L_7 = (JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA*)il2cpp_codegen_object_new(JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA_il2cpp_TypeInfo_var);
		NullCheck(L_7);
		JavaMethodCall_1__ctor_m23975F7FD0FE3A43155D75E53A95085F6504F9CD(L_7, __this, _stringLiteral28CD321C015FD8117CD1B202457F438D5669BE3C, JavaMethodCall_1__ctor_m23975F7FD0FE3A43155D75E53A95085F6504F9CD_RuntimeMethod_var);
		JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA* L_8 = L_7;
		FacebookDelegate_1_t7CA00F6A27B3FE85139590EFEA03B7C7C0D4A66D* L_9 = ___3_callback;
		NullCheck(L_8);
		MethodCall_1_set_Callback_m5ACC9D63CAC86405A106A94B35C11B8641FA2088_inline(L_8, L_9, MethodCall_1_set_Callback_m5ACC9D63CAC86405A106A94B35C11B8641FA2088_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_10 = V_0;
		NullCheck(L_8);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IMediaUploadResult>::Call(Facebook.Unity.MethodArguments) */, L_8, L_10);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::UploadVideoToMediaLibrary(System.String,System.Uri,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IMediaUploadResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_UploadVideoToMediaLibrary_m5957ADBC0C6413F14A6D19F6F5ECD4DBDC7AC6F0 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_caption, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___1_videoUri, FacebookDelegate_1_t7CA00F6A27B3FE85139590EFEA03B7C7C0D4A66D* ___2_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m23975F7FD0FE3A43155D75E53A95085F6504F9CD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m5ACC9D63CAC86405A106A94B35C11B8641FA2088_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5B63100E80DAFEE5BA4AF4EDCCB7370ED6550264);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral8412159A618D7E89197A238D90096386B02C395E);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE7399AD4B42F540AF0F4AD91938F8F943B39424E);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_caption;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteral5B63100E80DAFEE5BA4AF4EDCCB7370ED6550264, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_4 = ___1_videoUri;
		NullCheck(L_3);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_3, _stringLiteralE7399AD4B42F540AF0F4AD91938F8F943B39424E, L_4, NULL);
		JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA* L_5 = (JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA*)il2cpp_codegen_object_new(JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA_il2cpp_TypeInfo_var);
		NullCheck(L_5);
		JavaMethodCall_1__ctor_m23975F7FD0FE3A43155D75E53A95085F6504F9CD(L_5, __this, _stringLiteral8412159A618D7E89197A238D90096386B02C395E, JavaMethodCall_1__ctor_m23975F7FD0FE3A43155D75E53A95085F6504F9CD_RuntimeMethod_var);
		JavaMethodCall_1_t33A1BEECAD3E080C4A0A440633F49B963F14BEBA* L_6 = L_5;
		FacebookDelegate_1_t7CA00F6A27B3FE85139590EFEA03B7C7C0D4A66D* L_7 = ___2_callback;
		NullCheck(L_6);
		MethodCall_1_set_Callback_m5ACC9D63CAC86405A106A94B35C11B8641FA2088_inline(L_6, L_7, MethodCall_1_set_Callback_m5ACC9D63CAC86405A106A94B35C11B8641FA2088_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_8 = V_0;
		NullCheck(L_6);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IMediaUploadResult>::Call(Facebook.Unity.MethodArguments) */, L_6, L_8);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::OnIAPReady(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IIAPReadyResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_OnIAPReady_m3C93CF5FB07B3EEF6157C77C50D9E2A81DBB537F (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_tA2A619ACCF137D8094955A556E78A66749C43F74* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m98326A81B2D27D3EF9182B178B0B3B026C6A41E6_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m3AC34DDE1F2C7F926E3440F251C1519997E656B5_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral336EA4DDA4EC10AC44C546769AC33BC45133C712);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53* L_0 = (JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53*)il2cpp_codegen_object_new(JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_m98326A81B2D27D3EF9182B178B0B3B026C6A41E6(L_0, __this, _stringLiteral336EA4DDA4EC10AC44C546769AC33BC45133C712, JavaMethodCall_1__ctor_m98326A81B2D27D3EF9182B178B0B3B026C6A41E6_RuntimeMethod_var);
		JavaMethodCall_1_tE0999793606B26061643A0442BA9A0661CF27C53* L_1 = L_0;
		FacebookDelegate_1_tA2A619ACCF137D8094955A556E78A66749C43F74* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m3AC34DDE1F2C7F926E3440F251C1519997E656B5_inline(L_1, L_2, MethodCall_1_set_Callback_m3AC34DDE1F2C7F926E3440F251C1519997E656B5_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IIAPReadyResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::GetCatalog(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ICatalogResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_GetCatalog_m8BDF9EC7DEAAB0B764F5FC70C52A34ED9939BA7F (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_tA8F51BBA2E7364881368E0CBED892F561162C32E* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m88BEF0F791A57E9BAF9D3C26D3053BBD54F4BD8D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m0300E7B325CFF8E837D2D5B77E3B160C53C1068F_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralB7E2E157CB850ECDEF7FC79B85F39918BAC5A8DD);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A* L_0 = (JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A*)il2cpp_codegen_object_new(JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_m88BEF0F791A57E9BAF9D3C26D3053BBD54F4BD8D(L_0, __this, _stringLiteralB7E2E157CB850ECDEF7FC79B85F39918BAC5A8DD, JavaMethodCall_1__ctor_m88BEF0F791A57E9BAF9D3C26D3053BBD54F4BD8D_RuntimeMethod_var);
		JavaMethodCall_1_t45DBDE420B8AE0176C550FFA21D33F81D545986A* L_1 = L_0;
		FacebookDelegate_1_tA8F51BBA2E7364881368E0CBED892F561162C32E* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m0300E7B325CFF8E837D2D5B77E3B160C53C1068F_inline(L_1, L_2, MethodCall_1_set_Callback_m0300E7B325CFF8E837D2D5B77E3B160C53C1068F_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ICatalogResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::GetPurchases(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchasesResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_GetPurchases_m73D2A1110F5D5D5A548A42EFA8BF4FF2EB2713CC (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_t61D056F5D405CB3DF2F643449D9C03A64AB8E818* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mAC2E2E15420B915144E3A7324B6F901123C5BC22_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m490C4FB7E8194080AD5A53C8BA7F96134C1EED27_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral24187FD658C1381615A4926BBAEE466B99119C27);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D* L_0 = (JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D*)il2cpp_codegen_object_new(JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_mAC2E2E15420B915144E3A7324B6F901123C5BC22(L_0, __this, _stringLiteral24187FD658C1381615A4926BBAEE466B99119C27, JavaMethodCall_1__ctor_mAC2E2E15420B915144E3A7324B6F901123C5BC22_RuntimeMethod_var);
		JavaMethodCall_1_t82A6952A1D39DBE8EC5E806F3235F2D01A90927D* L_1 = L_0;
		FacebookDelegate_1_t61D056F5D405CB3DF2F643449D9C03A64AB8E818* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m490C4FB7E8194080AD5A53C8BA7F96134C1EED27_inline(L_1, L_2, MethodCall_1_set_Callback_m490C4FB7E8194080AD5A53C8BA7F96134C1EED27_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchasesResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::Purchase(System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPurchaseResult>,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_Purchase_mE61E80DAE920AB7E9C9D729A8F55C0B9997A2D9C (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_productID, FacebookDelegate_1_tBD274D4AED39A6A24A22981D38D3CBF447CF036E* ___1_callback, String_t* ___2_developerPayload, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m25A837E2CB92362A09A93D1260F3D010966A513E_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_mFFB10B41B3F6834AAF0A0A2D1EB306A89AA8E4A1_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1317CF02F3F3926703DF869C594244C35D400F6A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral364F4173856E05DF96506EB76D1DECAD55D36048);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralB599F7943E63846FF6287E29254EF871F7C11DD9);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_productID;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteral364F4173856E05DF96506EB76D1DECAD55D36048, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		String_t* L_4 = ___2_developerPayload;
		NullCheck(L_3);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_3, _stringLiteralB599F7943E63846FF6287E29254EF871F7C11DD9, L_4, NULL);
		JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7* L_5 = (JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7*)il2cpp_codegen_object_new(JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7_il2cpp_TypeInfo_var);
		NullCheck(L_5);
		JavaMethodCall_1__ctor_m25A837E2CB92362A09A93D1260F3D010966A513E(L_5, __this, _stringLiteral1317CF02F3F3926703DF869C594244C35D400F6A, JavaMethodCall_1__ctor_m25A837E2CB92362A09A93D1260F3D010966A513E_RuntimeMethod_var);
		JavaMethodCall_1_tF3D19E9E417716ABFC13D85142BFFB6E3C7B6CA7* L_6 = L_5;
		FacebookDelegate_1_tBD274D4AED39A6A24A22981D38D3CBF447CF036E* L_7 = ___1_callback;
		NullCheck(L_6);
		MethodCall_1_set_Callback_mFFB10B41B3F6834AAF0A0A2D1EB306A89AA8E4A1_inline(L_6, L_7, MethodCall_1_set_Callback_mFFB10B41B3F6834AAF0A0A2D1EB306A89AA8E4A1_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_8 = V_0;
		NullCheck(L_6);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IPurchaseResult>::Call(Facebook.Unity.MethodArguments) */, L_6, L_8);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::ConsumePurchase(System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IConsumePurchaseResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_ConsumePurchase_m2AD242A5189160D0F584A0B6CDF9EF92EAC2B1BF (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_purchaseToken, FacebookDelegate_1_tF96C838D4977CEFDB874E77F7A56B1CFECF4D8DD* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m5F7216810B33A4CCED764468B20F505B5449669D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m1725F4044DF470B54304044677371FA010FCA055_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral3DA85F3B75DC932851E7BA8C186F00E264675CBB);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralBFB25F06F09F6041E2BDD623C059ACE65A0FB1DA);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_purchaseToken;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteral3DA85F3B75DC932851E7BA8C186F00E264675CBB, L_2, NULL);
		JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB* L_3 = (JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB*)il2cpp_codegen_object_new(JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB_il2cpp_TypeInfo_var);
		NullCheck(L_3);
		JavaMethodCall_1__ctor_m5F7216810B33A4CCED764468B20F505B5449669D(L_3, __this, _stringLiteralBFB25F06F09F6041E2BDD623C059ACE65A0FB1DA, JavaMethodCall_1__ctor_m5F7216810B33A4CCED764468B20F505B5449669D_RuntimeMethod_var);
		JavaMethodCall_1_t2E89DD52C0F18A7848509B94E216943781ED04BB* L_4 = L_3;
		FacebookDelegate_1_tF96C838D4977CEFDB874E77F7A56B1CFECF4D8DD* L_5 = ___1_callback;
		NullCheck(L_4);
		MethodCall_1_set_Callback_m1725F4044DF470B54304044677371FA010FCA055_inline(L_4, L_5, MethodCall_1_set_Callback_m1725F4044DF470B54304044677371FA010FCA055_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		NullCheck(L_4);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IConsumePurchaseResult>::Call(Facebook.Unity.MethodArguments) */, L_4, L_6);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::InitCloudGame(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInitCloudGameResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_InitCloudGame_m48B0B85D69AC4272CFE86F24EE66146553A6355D (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_tB829CDA8266CAB15D1703F5904BB2F8592CA60E8* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m550304F60083DC63749F387B03DF9E7777F83265_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m80D0B98B0FF86073D4DE52A2FB8C56374851424A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5176DB50285A90A97F8930F5CF3EA329C7B78385);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8* L_0 = (JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8*)il2cpp_codegen_object_new(JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_m550304F60083DC63749F387B03DF9E7777F83265(L_0, __this, _stringLiteral5176DB50285A90A97F8930F5CF3EA329C7B78385, JavaMethodCall_1__ctor_m550304F60083DC63749F387B03DF9E7777F83265_RuntimeMethod_var);
		JavaMethodCall_1_tCC5339C83E63C8E1FD0CCA76AC6EBEC60C3905E8* L_1 = L_0;
		FacebookDelegate_1_tB829CDA8266CAB15D1703F5904BB2F8592CA60E8* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m80D0B98B0FF86073D4DE52A2FB8C56374851424A_inline(L_1, L_2, MethodCall_1_set_Callback_m80D0B98B0FF86073D4DE52A2FB8C56374851424A_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IInitCloudGameResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::ScheduleAppToUserNotification(System.String,System.String,System.Uri,System.Int32,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IScheduleAppToUserNotificationResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_ScheduleAppToUserNotification_mE2ABBC5F7486F7468D710680580F0DF2DA242A39 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_title, String_t* ___1_body, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___2_media, int32_t ___3_timeInterval, String_t* ___4_payload, FacebookDelegate_1_t623E520D5B99523D410AEE72AB9FF4901DC356AC* ___5_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m1A91A6D8302126249B270F478B851AE405105FEE_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m33FD8977CC0AFF79A6EBA5E2DE85109BDB68ED8F_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral55AC598ED5884D77F7D97920AA5DDDDA2CAA02B4);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral8F113FA4A67982F9646FA7415A8AD19E9077A12A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA4E9B4490477EACED46B6D66D6EFFA258D8C3D7A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC7B4D926EF9532A71B25AEC040A33D52C926425F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralCD002DD70C7AAC9CFF6D7D4821927E13D2989493);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF628EB600A45B99D481CC2B1F52A62CC1B8169AF);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_title;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralC7B4D926EF9532A71B25AEC040A33D52C926425F, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		String_t* L_4 = ___1_body;
		NullCheck(L_3);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_3, _stringLiteralCD002DD70C7AAC9CFF6D7D4821927E13D2989493, L_4, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_6 = ___2_media;
		NullCheck(L_5);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_5, _stringLiteralF628EB600A45B99D481CC2B1F52A62CC1B8169AF, L_6, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_7 = V_0;
		int32_t L_8 = ___3_timeInterval;
		NullCheck(L_7);
		MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC(L_7, _stringLiteral8F113FA4A67982F9646FA7415A8AD19E9077A12A, L_8, MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_9 = V_0;
		String_t* L_10 = ___4_payload;
		NullCheck(L_9);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_9, _stringLiteral55AC598ED5884D77F7D97920AA5DDDDA2CAA02B4, L_10, NULL);
		JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152* L_11 = (JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152*)il2cpp_codegen_object_new(JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152_il2cpp_TypeInfo_var);
		NullCheck(L_11);
		JavaMethodCall_1__ctor_m1A91A6D8302126249B270F478B851AE405105FEE(L_11, __this, _stringLiteralA4E9B4490477EACED46B6D66D6EFFA258D8C3D7A, JavaMethodCall_1__ctor_m1A91A6D8302126249B270F478B851AE405105FEE_RuntimeMethod_var);
		JavaMethodCall_1_t1D69EF08106315F02F16C2BD2F92283D45B7A152* L_12 = L_11;
		FacebookDelegate_1_t623E520D5B99523D410AEE72AB9FF4901DC356AC* L_13 = ___5_callback;
		NullCheck(L_12);
		MethodCall_1_set_Callback_m33FD8977CC0AFF79A6EBA5E2DE85109BDB68ED8F_inline(L_12, L_13, MethodCall_1_set_Callback_m33FD8977CC0AFF79A6EBA5E2DE85109BDB68ED8F_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_14 = V_0;
		NullCheck(L_12);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IScheduleAppToUserNotificationResult>::Call(Facebook.Unity.MethodArguments) */, L_12, L_14);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::LoadInterstitialAd(System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInterstitialAdResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_LoadInterstitialAd_mFABB93839D1D47D5BF1D65F612215F3341D762D0 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_placementID, FacebookDelegate_1_t6BAE034F2CC3270BFC282A9D0DEB58CC5F91C265* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m82480001BC5DA6A73E94BC8C41F4296473A9141F_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_mCE2EA397F7DFB68974A15A329A01050C09E92A61_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5850C06C770BC2AC6742A4BEF30C6D430FC07F2E);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralD4CCCB463309EDE01997B4A25530A4F9D64B0BAF);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_placementID;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralD4CCCB463309EDE01997B4A25530A4F9D64B0BAF, L_2, NULL);
		JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466* L_3 = (JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466*)il2cpp_codegen_object_new(JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466_il2cpp_TypeInfo_var);
		NullCheck(L_3);
		JavaMethodCall_1__ctor_m82480001BC5DA6A73E94BC8C41F4296473A9141F(L_3, __this, _stringLiteral5850C06C770BC2AC6742A4BEF30C6D430FC07F2E, JavaMethodCall_1__ctor_m82480001BC5DA6A73E94BC8C41F4296473A9141F_RuntimeMethod_var);
		JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466* L_4 = L_3;
		FacebookDelegate_1_t6BAE034F2CC3270BFC282A9D0DEB58CC5F91C265* L_5 = ___1_callback;
		NullCheck(L_4);
		MethodCall_1_set_Callback_mCE2EA397F7DFB68974A15A329A01050C09E92A61_inline(L_4, L_5, MethodCall_1_set_Callback_mCE2EA397F7DFB68974A15A329A01050C09E92A61_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		NullCheck(L_4);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IInterstitialAdResult>::Call(Facebook.Unity.MethodArguments) */, L_4, L_6);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::ShowInterstitialAd(System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IInterstitialAdResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_ShowInterstitialAd_mCC55F1D6B6FB59B226AAD6D1767C346A3111209E (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_placementID, FacebookDelegate_1_t6BAE034F2CC3270BFC282A9D0DEB58CC5F91C265* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m82480001BC5DA6A73E94BC8C41F4296473A9141F_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_mCE2EA397F7DFB68974A15A329A01050C09E92A61_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4CEA8A0063213A8412FA6B1C943CA05E38FD880E);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralD4CCCB463309EDE01997B4A25530A4F9D64B0BAF);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_placementID;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralD4CCCB463309EDE01997B4A25530A4F9D64B0BAF, L_2, NULL);
		JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466* L_3 = (JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466*)il2cpp_codegen_object_new(JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466_il2cpp_TypeInfo_var);
		NullCheck(L_3);
		JavaMethodCall_1__ctor_m82480001BC5DA6A73E94BC8C41F4296473A9141F(L_3, __this, _stringLiteral4CEA8A0063213A8412FA6B1C943CA05E38FD880E, JavaMethodCall_1__ctor_m82480001BC5DA6A73E94BC8C41F4296473A9141F_RuntimeMethod_var);
		JavaMethodCall_1_t1F88D42CE635074D1A4F1E5F436E1E85538A8466* L_4 = L_3;
		FacebookDelegate_1_t6BAE034F2CC3270BFC282A9D0DEB58CC5F91C265* L_5 = ___1_callback;
		NullCheck(L_4);
		MethodCall_1_set_Callback_mCE2EA397F7DFB68974A15A329A01050C09E92A61_inline(L_4, L_5, MethodCall_1_set_Callback_mCE2EA397F7DFB68974A15A329A01050C09E92A61_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		NullCheck(L_4);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IInterstitialAdResult>::Call(Facebook.Unity.MethodArguments) */, L_4, L_6);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::LoadRewardedVideo(System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IRewardedVideoResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_LoadRewardedVideo_mC13C361164376E76BB38CAA0D5692552816B5754 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_placementID, FacebookDelegate_1_t9C5748E8AC36242F3F03171AA1E6306E60860ACD* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mF67A248E040456EE62DDE37D0E7A8BE10AFFD665_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m754DB9950904B1CCB5076A82982D93946AF22BE3_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral81B5C6B2E03435106B6CBAD2722668F8D5FC2913);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralD4CCCB463309EDE01997B4A25530A4F9D64B0BAF);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_placementID;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralD4CCCB463309EDE01997B4A25530A4F9D64B0BAF, L_2, NULL);
		JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20* L_3 = (JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20*)il2cpp_codegen_object_new(JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20_il2cpp_TypeInfo_var);
		NullCheck(L_3);
		JavaMethodCall_1__ctor_mF67A248E040456EE62DDE37D0E7A8BE10AFFD665(L_3, __this, _stringLiteral81B5C6B2E03435106B6CBAD2722668F8D5FC2913, JavaMethodCall_1__ctor_mF67A248E040456EE62DDE37D0E7A8BE10AFFD665_RuntimeMethod_var);
		JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20* L_4 = L_3;
		FacebookDelegate_1_t9C5748E8AC36242F3F03171AA1E6306E60860ACD* L_5 = ___1_callback;
		NullCheck(L_4);
		MethodCall_1_set_Callback_m754DB9950904B1CCB5076A82982D93946AF22BE3_inline(L_4, L_5, MethodCall_1_set_Callback_m754DB9950904B1CCB5076A82982D93946AF22BE3_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		NullCheck(L_4);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IRewardedVideoResult>::Call(Facebook.Unity.MethodArguments) */, L_4, L_6);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::ShowRewardedVideo(System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IRewardedVideoResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_ShowRewardedVideo_m69BF5BB105E609C0DCFAB93F53A1FB71B7BD7817 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_placementID, FacebookDelegate_1_t9C5748E8AC36242F3F03171AA1E6306E60860ACD* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mF67A248E040456EE62DDE37D0E7A8BE10AFFD665_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m754DB9950904B1CCB5076A82982D93946AF22BE3_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral6EA2D6BC4B09ED43616195D526B011F0DC643ECB);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralD4CCCB463309EDE01997B4A25530A4F9D64B0BAF);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_placementID;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralD4CCCB463309EDE01997B4A25530A4F9D64B0BAF, L_2, NULL);
		JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20* L_3 = (JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20*)il2cpp_codegen_object_new(JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20_il2cpp_TypeInfo_var);
		NullCheck(L_3);
		JavaMethodCall_1__ctor_mF67A248E040456EE62DDE37D0E7A8BE10AFFD665(L_3, __this, _stringLiteral6EA2D6BC4B09ED43616195D526B011F0DC643ECB, JavaMethodCall_1__ctor_mF67A248E040456EE62DDE37D0E7A8BE10AFFD665_RuntimeMethod_var);
		JavaMethodCall_1_tA0A5949750E46ECE77FD3ECAE3F060DB7564BC20* L_4 = L_3;
		FacebookDelegate_1_t9C5748E8AC36242F3F03171AA1E6306E60860ACD* L_5 = ___1_callback;
		NullCheck(L_4);
		MethodCall_1_set_Callback_m754DB9950904B1CCB5076A82982D93946AF22BE3_inline(L_4, L_5, MethodCall_1_set_Callback_m754DB9950904B1CCB5076A82982D93946AF22BE3_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		NullCheck(L_4);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IRewardedVideoResult>::Call(Facebook.Unity.MethodArguments) */, L_4, L_6);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::GetPayload(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayloadResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_GetPayload_mB059A54B0859D8768F4C964C6D9D0842A3F71C7E (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_tB6B04FA63685B9A0D41AB08E3E5BC96A72D7A1D7* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mA93CFE9895EE4033CEE85613734423E1E8870310_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m7C7EFBA28D827AB9DEE7946EBD20C87AD1EB44C5_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF4A79D39206648981A640057F84D12739679216F);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7* L_0 = (JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7*)il2cpp_codegen_object_new(JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_mA93CFE9895EE4033CEE85613734423E1E8870310(L_0, __this, _stringLiteralF4A79D39206648981A640057F84D12739679216F, JavaMethodCall_1__ctor_mA93CFE9895EE4033CEE85613734423E1E8870310_RuntimeMethod_var);
		JavaMethodCall_1_tC6A232991D093EB49C24FA4A9C9D03AF55FB7CE7* L_1 = L_0;
		FacebookDelegate_1_tB6B04FA63685B9A0D41AB08E3E5BC96A72D7A1D7* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m7C7EFBA28D827AB9DEE7946EBD20C87AD1EB44C5_inline(L_1, L_2, MethodCall_1_set_Callback_m7C7EFBA28D827AB9DEE7946EBD20C87AD1EB44C5_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IPayloadResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::PostSessionScore(System.Int32,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ISessionScoreResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_PostSessionScore_m6AFF1D4CDBB0AFB9C0E6781992A8D2F9B37D96ED (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, int32_t ___0_score, FacebookDelegate_1_t610D359C8E47669CC0D9F0B6CA9DF9240BF80267* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mCD10E054583159BC1950612666A16FD33761CBE4_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_mE524DA942B4350B09ECDF58D3FA0D6D3765CAE4D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1CF8013125B2A6F1169557DB315E42C0E45A87C5);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC0E2DE04AE40B3B0493F0F846F34B279C6D44FE9);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2;
		L_2 = Int32_ToString_m030E01C24E294D6762FB0B6F37CB541581F55CA5((&___0_score), NULL);
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralC0E2DE04AE40B3B0493F0F846F34B279C6D44FE9, L_2, NULL);
		JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862* L_3 = (JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862*)il2cpp_codegen_object_new(JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862_il2cpp_TypeInfo_var);
		NullCheck(L_3);
		JavaMethodCall_1__ctor_mCD10E054583159BC1950612666A16FD33761CBE4(L_3, __this, _stringLiteral1CF8013125B2A6F1169557DB315E42C0E45A87C5, JavaMethodCall_1__ctor_mCD10E054583159BC1950612666A16FD33761CBE4_RuntimeMethod_var);
		JavaMethodCall_1_t9B814BC3EF4BDA4A29549C6F8210A62F33C19862* L_4 = L_3;
		FacebookDelegate_1_t610D359C8E47669CC0D9F0B6CA9DF9240BF80267* L_5 = ___1_callback;
		NullCheck(L_4);
		MethodCall_1_set_Callback_mE524DA942B4350B09ECDF58D3FA0D6D3765CAE4D_inline(L_4, L_5, MethodCall_1_set_Callback_mE524DA942B4350B09ECDF58D3FA0D6D3765CAE4D_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		NullCheck(L_4);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ISessionScoreResult>::Call(Facebook.Unity.MethodArguments) */, L_4, L_6);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::PostTournamentScore(System.Int32,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentScoreResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_PostTournamentScore_m9D81CEBBAC153093014752F6501EB0B228AEC890 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, int32_t ___0_score, FacebookDelegate_1_t474682078C474498C8D4805F13E9077763B39255* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mBC1E3309CE7ADE92E369530993D7A36F5EFEF649_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m695883E994C52B5CB6C0AB3939ED902B2F1455A5_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral817EC3071B8F4D9C6EB59BDACEFB3E2CA37BD9F0);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC0E2DE04AE40B3B0493F0F846F34B279C6D44FE9);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2;
		L_2 = Int32_ToString_m030E01C24E294D6762FB0B6F37CB541581F55CA5((&___0_score), NULL);
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralC0E2DE04AE40B3B0493F0F846F34B279C6D44FE9, L_2, NULL);
		JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557* L_3 = (JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557*)il2cpp_codegen_object_new(JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557_il2cpp_TypeInfo_var);
		NullCheck(L_3);
		JavaMethodCall_1__ctor_mBC1E3309CE7ADE92E369530993D7A36F5EFEF649(L_3, __this, _stringLiteral817EC3071B8F4D9C6EB59BDACEFB3E2CA37BD9F0, JavaMethodCall_1__ctor_mBC1E3309CE7ADE92E369530993D7A36F5EFEF649_RuntimeMethod_var);
		JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557* L_4 = L_3;
		FacebookDelegate_1_t474682078C474498C8D4805F13E9077763B39255* L_5 = ___1_callback;
		NullCheck(L_4);
		MethodCall_1_set_Callback_m695883E994C52B5CB6C0AB3939ED902B2F1455A5_inline(L_4, L_5, MethodCall_1_set_Callback_m695883E994C52B5CB6C0AB3939ED902B2F1455A5_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_6 = V_0;
		NullCheck(L_4);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentScoreResult>::Call(Facebook.Unity.MethodArguments) */, L_4, L_6);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::GetTournament(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_GetTournament_m53D7AB0908FDD98CEF08F5DC12C1F5BDEE5D4DD3 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_tD9C9BBFE6AD0931C935E391D858C37BEBF14BBA3* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m633D3D2232A91D7FBE7FA9F6E87212EC1F47708C_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m31B32945ADD64F439F695CD2B613934BEFCC15A8_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral6D79C4D8A34AE7B74EBCED100A562B73086EA268);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71* L_0 = (JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71*)il2cpp_codegen_object_new(JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_m633D3D2232A91D7FBE7FA9F6E87212EC1F47708C(L_0, __this, _stringLiteral6D79C4D8A34AE7B74EBCED100A562B73086EA268, JavaMethodCall_1__ctor_m633D3D2232A91D7FBE7FA9F6E87212EC1F47708C_RuntimeMethod_var);
		JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71* L_1 = L_0;
		FacebookDelegate_1_tD9C9BBFE6AD0931C935E391D858C37BEBF14BBA3* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m31B32945ADD64F439F695CD2B613934BEFCC15A8_inline(L_1, L_2, MethodCall_1_set_Callback_m31B32945ADD64F439F695CD2B613934BEFCC15A8_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::CreateTournament(System.Int32,System.String,System.String,System.String,System.String,System.Collections.Generic.Dictionary`2<System.String,System.String>,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_CreateTournament_mC04A8889339BD907399E759E37E9C80EC591B43C (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, int32_t ___0_initialScore, String_t* ___1_title, String_t* ___2_imageBase64DataUrl, String_t* ___3_sortOrder, String_t* ___4_scoreFormat, Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* ___5_data, FacebookDelegate_1_tD9C9BBFE6AD0931C935E391D858C37BEBF14BBA3* ___6_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Enumerable_ToDictionary_TisKeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A_TisString_t_TisRuntimeObject_m1565E67B7F63889DEE95110A76BCB790092DD21B_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_m633D3D2232A91D7FBE7FA9F6E87212EC1F47708C_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m31B32945ADD64F439F695CD2B613934BEFCC15A8_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_U3CCreateTournamentU3Eb__65_0_mFA14522DDB5C630904E8C38ABBD1D9345954954B_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_U3CCreateTournamentU3Eb__65_1_mE9A32D06A70E684E33F841470131880439A05509_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral52C66E4B826647F480AEE3DD7DBC5996EC331410);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral558B14189B5200EFA26DCE8019A608B009BE50D4);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral65D8A29B27547180FAA9AF5295FC58F78CE0FEBD);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral7B8D306B8DAE57DC7D3095E04C9C6A2BFE3E7A4F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC7B4D926EF9532A71B25AEC040A33D52C926425F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralEC7F65B35829D8281484B259860DA096F9BAD474);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* G_B2_0 = NULL;
	Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* G_B2_1 = NULL;
	String_t* G_B2_2 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B2_3 = NULL;
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* G_B1_0 = NULL;
	Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* G_B1_1 = NULL;
	String_t* G_B1_2 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B1_3 = NULL;
	Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* G_B4_0 = NULL;
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* G_B4_1 = NULL;
	Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* G_B4_2 = NULL;
	String_t* G_B4_3 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B4_4 = NULL;
	Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* G_B3_0 = NULL;
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* G_B3_1 = NULL;
	Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* G_B3_2 = NULL;
	String_t* G_B3_3 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B3_4 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2;
		L_2 = Int32_ToString_m030E01C24E294D6762FB0B6F37CB541581F55CA5((&___0_initialScore), NULL);
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralEC7F65B35829D8281484B259860DA096F9BAD474, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		String_t* L_4 = ___1_title;
		NullCheck(L_3);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_3, _stringLiteralC7B4D926EF9532A71B25AEC040A33D52C926425F, L_4, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		String_t* L_6 = ___2_imageBase64DataUrl;
		NullCheck(L_5);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_5, _stringLiteral52C66E4B826647F480AEE3DD7DBC5996EC331410, L_6, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_7 = V_0;
		String_t* L_8 = ___3_sortOrder;
		NullCheck(L_7);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_7, _stringLiteral7B8D306B8DAE57DC7D3095E04C9C6A2BFE3E7A4F, L_8, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_9 = V_0;
		String_t* L_10 = ___4_scoreFormat;
		NullCheck(L_9);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_9, _stringLiteral558B14189B5200EFA26DCE8019A608B009BE50D4, L_10, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_11 = V_0;
		Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* L_12 = ___5_data;
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* L_13 = ((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__65_0_1;
		Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* L_14 = L_13;
		G_B1_0 = L_14;
		G_B1_1 = L_12;
		G_B1_2 = _stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469;
		G_B1_3 = L_11;
		if (L_14)
		{
			G_B2_0 = L_14;
			G_B2_1 = L_12;
			G_B2_2 = _stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469;
			G_B2_3 = L_11;
			goto IL_0071;
		}
	}
	{
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* L_15 = ((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9_0;
		Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* L_16 = (Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088*)il2cpp_codegen_object_new(Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088_il2cpp_TypeInfo_var);
		NullCheck(L_16);
		Func_2__ctor_m48BD5538630AB90CAACF2ADC165985AB743A6C30(L_16, L_15, (intptr_t)((void*)U3CU3Ec_U3CCreateTournamentU3Eb__65_0_mFA14522DDB5C630904E8C38ABBD1D9345954954B_RuntimeMethod_var), NULL);
		Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* L_17 = L_16;
		((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__65_0_1 = L_17;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__65_0_1), (void*)L_17);
		G_B2_0 = L_17;
		G_B2_1 = G_B1_1;
		G_B2_2 = G_B1_2;
		G_B2_3 = G_B1_3;
	}

IL_0071:
	{
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* L_18 = ((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__65_1_2;
		Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* L_19 = L_18;
		G_B3_0 = L_19;
		G_B3_1 = G_B2_0;
		G_B3_2 = G_B2_1;
		G_B3_3 = G_B2_2;
		G_B3_4 = G_B2_3;
		if (L_19)
		{
			G_B4_0 = L_19;
			G_B4_1 = G_B2_0;
			G_B4_2 = G_B2_1;
			G_B4_3 = G_B2_2;
			G_B4_4 = G_B2_3;
			goto IL_0090;
		}
	}
	{
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* L_20 = ((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9_0;
		Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* L_21 = (Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF*)il2cpp_codegen_object_new(Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF_il2cpp_TypeInfo_var);
		NullCheck(L_21);
		Func_2__ctor_m554C9E1DBFF27492E948AA1DBB4446038F0EB728(L_21, L_20, (intptr_t)((void*)U3CU3Ec_U3CCreateTournamentU3Eb__65_1_mE9A32D06A70E684E33F841470131880439A05509_RuntimeMethod_var), NULL);
		Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* L_22 = L_21;
		((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__65_1_2 = L_22;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__65_1_2), (void*)L_22);
		G_B4_0 = L_22;
		G_B4_1 = G_B3_1;
		G_B4_2 = G_B3_2;
		G_B4_3 = G_B3_3;
		G_B4_4 = G_B3_4;
	}

IL_0090:
	{
		Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* L_23;
		L_23 = Enumerable_ToDictionary_TisKeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A_TisString_t_TisRuntimeObject_m1565E67B7F63889DEE95110A76BCB790092DD21B(G_B4_2, G_B4_1, G_B4_0, Enumerable_ToDictionary_TisKeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A_TisString_t_TisRuntimeObject_m1565E67B7F63889DEE95110A76BCB790092DD21B_RuntimeMethod_var);
		NullCheck(G_B4_4);
		MethodArguments_AddDictionary_m1633D0C3C4E5262F1F846414317BEED6C32EB75C(G_B4_4, G_B4_3, L_23, NULL);
		JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71* L_24 = (JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71*)il2cpp_codegen_object_new(JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71_il2cpp_TypeInfo_var);
		NullCheck(L_24);
		JavaMethodCall_1__ctor_m633D3D2232A91D7FBE7FA9F6E87212EC1F47708C(L_24, __this, _stringLiteral65D8A29B27547180FAA9AF5295FC58F78CE0FEBD, JavaMethodCall_1__ctor_m633D3D2232A91D7FBE7FA9F6E87212EC1F47708C_RuntimeMethod_var);
		JavaMethodCall_1_t5A7EBA3E0452292DEA2F394299039038EF32AA71* L_25 = L_24;
		FacebookDelegate_1_tD9C9BBFE6AD0931C935E391D858C37BEBF14BBA3* L_26 = ___6_callback;
		NullCheck(L_25);
		MethodCall_1_set_Callback_m31B32945ADD64F439F695CD2B613934BEFCC15A8_inline(L_25, L_26, MethodCall_1_set_Callback_m31B32945ADD64F439F695CD2B613934BEFCC15A8_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_27 = V_0;
		NullCheck(L_25);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentResult>::Call(Facebook.Unity.MethodArguments) */, L_25, L_27);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::ShareTournament(System.Collections.Generic.Dictionary`2<System.String,System.String>,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ITournamentScoreResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_ShareTournament_m22EC3A3D73007687AB4FEB9D732BCBC41BC52B9D (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* ___0_data, FacebookDelegate_1_t474682078C474498C8D4805F13E9077763B39255* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Enumerable_ToDictionary_TisKeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A_TisString_t_TisRuntimeObject_m1565E67B7F63889DEE95110A76BCB790092DD21B_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mBC1E3309CE7ADE92E369530993D7A36F5EFEF649_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m695883E994C52B5CB6C0AB3939ED902B2F1455A5_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_U3CShareTournamentU3Eb__66_0_mA62839F14645C8D8049BAD3091FA31EFE8C31ACE_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_U3CShareTournamentU3Eb__66_1_m86A8DF73410D88DEE63BE781B5D30F7E3240D92A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1603A385DE0A7E3A96A39790B8CB10F554D4C836);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* G_B2_0 = NULL;
	Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* G_B2_1 = NULL;
	String_t* G_B2_2 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B2_3 = NULL;
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* G_B1_0 = NULL;
	Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* G_B1_1 = NULL;
	String_t* G_B1_2 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B1_3 = NULL;
	Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* G_B4_0 = NULL;
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* G_B4_1 = NULL;
	Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* G_B4_2 = NULL;
	String_t* G_B4_3 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B4_4 = NULL;
	Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* G_B3_0 = NULL;
	Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* G_B3_1 = NULL;
	Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* G_B3_2 = NULL;
	String_t* G_B3_3 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B3_4 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* L_2 = ___0_data;
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* L_3 = ((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__66_0_3;
		Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* L_4 = L_3;
		G_B1_0 = L_4;
		G_B1_1 = L_2;
		G_B1_2 = _stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469;
		G_B1_3 = L_1;
		if (L_4)
		{
			G_B2_0 = L_4;
			G_B2_1 = L_2;
			G_B2_2 = _stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469;
			G_B2_3 = L_1;
			goto IL_002c;
		}
	}
	{
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* L_5 = ((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9_0;
		Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* L_6 = (Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088*)il2cpp_codegen_object_new(Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088_il2cpp_TypeInfo_var);
		NullCheck(L_6);
		Func_2__ctor_m48BD5538630AB90CAACF2ADC165985AB743A6C30(L_6, L_5, (intptr_t)((void*)U3CU3Ec_U3CShareTournamentU3Eb__66_0_mA62839F14645C8D8049BAD3091FA31EFE8C31ACE_RuntimeMethod_var), NULL);
		Func_2_t0FD9221539E762B3867B2E3B6D6B3F90C6483088* L_7 = L_6;
		((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__66_0_3 = L_7;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__66_0_3), (void*)L_7);
		G_B2_0 = L_7;
		G_B2_1 = G_B1_1;
		G_B2_2 = G_B1_2;
		G_B2_3 = G_B1_3;
	}

IL_002c:
	{
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* L_8 = ((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__66_1_4;
		Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* L_9 = L_8;
		G_B3_0 = L_9;
		G_B3_1 = G_B2_0;
		G_B3_2 = G_B2_1;
		G_B3_3 = G_B2_2;
		G_B3_4 = G_B2_3;
		if (L_9)
		{
			G_B4_0 = L_9;
			G_B4_1 = G_B2_0;
			G_B4_2 = G_B2_1;
			G_B4_3 = G_B2_2;
			G_B4_4 = G_B2_3;
			goto IL_004b;
		}
	}
	{
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* L_10 = ((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9_0;
		Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* L_11 = (Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF*)il2cpp_codegen_object_new(Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF_il2cpp_TypeInfo_var);
		NullCheck(L_11);
		Func_2__ctor_m554C9E1DBFF27492E948AA1DBB4446038F0EB728(L_11, L_10, (intptr_t)((void*)U3CU3Ec_U3CShareTournamentU3Eb__66_1_m86A8DF73410D88DEE63BE781B5D30F7E3240D92A_RuntimeMethod_var), NULL);
		Func_2_t18AACF5209C6D5DAEF4AF2CD0276805A038EC0EF* L_12 = L_11;
		((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__66_1_4 = L_12;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9__66_1_4), (void*)L_12);
		G_B4_0 = L_12;
		G_B4_1 = G_B3_1;
		G_B4_2 = G_B3_2;
		G_B4_3 = G_B3_3;
		G_B4_4 = G_B3_4;
	}

IL_004b:
	{
		Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* L_13;
		L_13 = Enumerable_ToDictionary_TisKeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A_TisString_t_TisRuntimeObject_m1565E67B7F63889DEE95110A76BCB790092DD21B(G_B4_2, G_B4_1, G_B4_0, Enumerable_ToDictionary_TisKeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A_TisString_t_TisRuntimeObject_m1565E67B7F63889DEE95110A76BCB790092DD21B_RuntimeMethod_var);
		NullCheck(G_B4_4);
		MethodArguments_AddDictionary_m1633D0C3C4E5262F1F846414317BEED6C32EB75C(G_B4_4, G_B4_3, L_13, NULL);
		JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557* L_14 = (JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557*)il2cpp_codegen_object_new(JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557_il2cpp_TypeInfo_var);
		NullCheck(L_14);
		JavaMethodCall_1__ctor_mBC1E3309CE7ADE92E369530993D7A36F5EFEF649(L_14, __this, _stringLiteral1603A385DE0A7E3A96A39790B8CB10F554D4C836, JavaMethodCall_1__ctor_mBC1E3309CE7ADE92E369530993D7A36F5EFEF649_RuntimeMethod_var);
		JavaMethodCall_1_tB9779546C2D6F94874529873A9D09FB253328557* L_15 = L_14;
		FacebookDelegate_1_t474682078C474498C8D4805F13E9077763B39255* L_16 = ___1_callback;
		NullCheck(L_15);
		MethodCall_1_set_Callback_m695883E994C52B5CB6C0AB3939ED902B2F1455A5_inline(L_15, L_16, MethodCall_1_set_Callback_m695883E994C52B5CB6C0AB3939ED902B2F1455A5_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_17 = V_0;
		NullCheck(L_15);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.ITournamentScoreResult>::Call(Facebook.Unity.MethodArguments) */, L_15, L_17);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::OpenAppStore(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IOpenAppStoreResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_OpenAppStore_m76FBF23B9117A0CC7DDB59D5183F341855A05278 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, FacebookDelegate_1_t5E457BF6B03D4AD72D97F1848A0612B675747CFD* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1__ctor_mD281D9CF6AA50E4AF6D86C8046B972E4759C82E0_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m37167CE00F34CC6A9A24918CB34160CB9ECA017E_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA1BD4986FCB7DB3734997A8A31846021B8C0D54E);
		s_Il2CppMethodInitialized = true;
	}
	{
		JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5* L_0 = (JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5*)il2cpp_codegen_object_new(JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		JavaMethodCall_1__ctor_mD281D9CF6AA50E4AF6D86C8046B972E4759C82E0(L_0, __this, _stringLiteralA1BD4986FCB7DB3734997A8A31846021B8C0D54E, JavaMethodCall_1__ctor_mD281D9CF6AA50E4AF6D86C8046B972E4759C82E0_RuntimeMethod_var);
		JavaMethodCall_1_tA57D00A0AD086004049B97A807B8A08BB97F5FF5* L_1 = L_0;
		FacebookDelegate_1_t5E457BF6B03D4AD72D97F1848A0612B675747CFD* L_2 = ___0_callback;
		NullCheck(L_1);
		MethodCall_1_set_Callback_m37167CE00F34CC6A9A24918CB34160CB9ECA017E_inline(L_1, L_2, MethodCall_1_set_Callback_m37167CE00F34CC6A9A24918CB34160CB9ECA017E_RuntimeMethod_var);
		NullCheck(L_1);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IOpenAppStoreResult>::Call(Facebook.Unity.MethodArguments) */, L_1, (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::SetShareDialogMode(Facebook.Unity.ShareDialogMode)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_SetShareDialogMode_m0777C8F927E921C12CFE49DCFB12A977CC760DAD (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, int32_t ___0_mode, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ShareDialogMode_t06BC7B4180D56064F766F1F7D505594B59279523_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral76BA0539B6046ABC30566265F439F0062F640CC0);
		s_Il2CppMethodInitialized = true;
	}
	{
		Il2CppFakeBox<int32_t> L_0(ShareDialogMode_t06BC7B4180D56064F766F1F7D505594B59279523_il2cpp_TypeInfo_var, (&___0_mode));
		String_t* L_1;
		L_1 = Enum_ToString_m946B0B83C4470457D0FF555D862022C72BB55741((Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2*)(&L_0), NULL);
		AndroidFacebook_CallFB_mBFD340DDC8C5FD7AD5B9498EC31A03C371448889(__this, _stringLiteral76BA0539B6046ABC30566265F439F0062F640CC0, L_1, NULL);
		return;
	}
}
// Facebook.Unity.Mobile.Android.IAndroidWrapper Facebook.Unity.Mobile.Android.AndroidFacebook::GetAndroidWrapper()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* AndroidFacebook_GetAndroidWrapper_mFB879F02BAD046A4C58F0A4AFCADE56BB98673A9 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidWrapper_t3D8FC6369EBCAB7A86AE6A71C08CD96C9E1DDC63_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA8C42238AB7DF269244EE10586CF4DE50612AAA3);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralDF6F6437AE9DE592DED819C9D7AFC073AD780F19);
		s_Il2CppMethodInitialized = true;
	}
	{
		Assembly_t* L_0;
		L_0 = Assembly_Load_mC42733BACCA273EEAA32A341CBF53722A44DCC90(_stringLiteralDF6F6437AE9DE592DED819C9D7AFC073AD780F19, NULL);
		NullCheck(L_0);
		Type_t* L_1;
		L_1 = VirtualFuncInvoker1< Type_t*, String_t* >::Invoke(19 /* System.Type System.Reflection.Assembly::GetType(System.String) */, L_0, _stringLiteralA8C42238AB7DF269244EE10586CF4DE50612AAA3);
		RuntimeObject* L_2;
		L_2 = Activator_CreateInstance_mFF030428C64FDDFACC74DFAC97388A1C628BFBCF(L_1, NULL);
		return ((RuntimeObject*)Castclass((RuntimeObject*)L_2, IAndroidWrapper_t3D8FC6369EBCAB7A86AE6A71C08CD96C9E1DDC63_il2cpp_TypeInfo_var));
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook::CallFB(System.String,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebook_CallFB_mBFD340DDC8C5FD7AD5B9498EC31A03C371448889 (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_method, String_t* ___1_args, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidWrapper_t3D8FC6369EBCAB7A86AE6A71C08CD96C9E1DDC63_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___androidWrapper_5;
		String_t* L_1 = ___0_method;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_3 = L_2;
		String_t* L_4 = ___1_args;
		NullCheck(L_3);
		ArrayElementTypeCheck (L_3, L_4);
		(L_3)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_4);
		NullCheck(L_0);
		InterfaceActionInvoker2< String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* >::Invoke(1 /* System.Void Facebook.Unity.Mobile.Android.IAndroidWrapper::CallStatic(System.String,System.Object[]) */, IAndroidWrapper_t3D8FC6369EBCAB7A86AE6A71C08CD96C9E1DDC63_il2cpp_TypeInfo_var, L_0, L_1, L_3);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::.cctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__cctor_mBD1EE586906B8B42D0D4677D3664DE6F51CE820D (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* L_0 = (U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD*)il2cpp_codegen_object_new(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		U3CU3Ec__ctor_m6FA067A84B1AE0399E7ED35538573D3678CC0B38(L_0, NULL);
		((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD_il2cpp_TypeInfo_var))->___U3CU3E9_0), (void*)L_0);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__ctor_m6FA067A84B1AE0399E7ED35538573D3678CC0B38 (U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		return;
	}
}
// System.String Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::<CreateTournament>b__65_0(System.Collections.Generic.KeyValuePair`2<System.String,System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* U3CU3Ec_U3CCreateTournamentU3Eb__65_0_mFA14522DDB5C630904E8C38ABBD1D9345954954B (U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* __this, KeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A ___0_pair, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&KeyValuePair_2_get_Key_m654BCCAE2F20CB11D8E8C2D2C886A0C8A13EB1C4_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0;
		L_0 = KeyValuePair_2_get_Key_m654BCCAE2F20CB11D8E8C2D2C886A0C8A13EB1C4_inline((&___0_pair), KeyValuePair_2_get_Key_m654BCCAE2F20CB11D8E8C2D2C886A0C8A13EB1C4_RuntimeMethod_var);
		return L_0;
	}
}
// System.Object Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::<CreateTournament>b__65_1(System.Collections.Generic.KeyValuePair`2<System.String,System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* U3CU3Ec_U3CCreateTournamentU3Eb__65_1_mE9A32D06A70E684E33F841470131880439A05509 (U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* __this, KeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A ___0_pair, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&KeyValuePair_2_get_Value_m7345512A32CB4DCAA0643050B18DC8DCD71B927A_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0;
		L_0 = KeyValuePair_2_get_Value_m7345512A32CB4DCAA0643050B18DC8DCD71B927A_inline((&___0_pair), KeyValuePair_2_get_Value_m7345512A32CB4DCAA0643050B18DC8DCD71B927A_RuntimeMethod_var);
		return L_0;
	}
}
// System.String Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::<ShareTournament>b__66_0(System.Collections.Generic.KeyValuePair`2<System.String,System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* U3CU3Ec_U3CShareTournamentU3Eb__66_0_mA62839F14645C8D8049BAD3091FA31EFE8C31ACE (U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* __this, KeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A ___0_pair, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&KeyValuePair_2_get_Key_m654BCCAE2F20CB11D8E8C2D2C886A0C8A13EB1C4_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0;
		L_0 = KeyValuePair_2_get_Key_m654BCCAE2F20CB11D8E8C2D2C886A0C8A13EB1C4_inline((&___0_pair), KeyValuePair_2_get_Key_m654BCCAE2F20CB11D8E8C2D2C886A0C8A13EB1C4_RuntimeMethod_var);
		return L_0;
	}
}
// System.Object Facebook.Unity.Mobile.Android.AndroidFacebook/<>c::<ShareTournament>b__66_1(System.Collections.Generic.KeyValuePair`2<System.String,System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* U3CU3Ec_U3CShareTournamentU3Eb__66_1_m86A8DF73410D88DEE63BE781B5D30F7E3240D92A (U3CU3Ec_tA735FA2125E48AFF6D822CDC474E539DC0B9CFCD* __this, KeyValuePair_2_t47AB280304B50F542FD7E14F25DB2C374AEDD80A ___0_pair, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&KeyValuePair_2_get_Value_m7345512A32CB4DCAA0643050B18DC8DCD71B927A_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		String_t* L_0;
		L_0 = KeyValuePair_2_get_Value_m7345512A32CB4DCAA0643050B18DC8DCD71B927A_inline((&___0_pair), KeyValuePair_2_get_Value_m7345512A32CB4DCAA0643050B18DC8DCD71B927A_RuntimeMethod_var);
		return L_0;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebookGameObject::OnAwake()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebookGameObject_OnAwake_mB286A5F4810D9949241D37DC435258D6AEF21EF8 (AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* __this, const RuntimeMethod* method) 
{
	{
		CodelessIAPAutoLog_addListenerToIAPButtons_mF59A2E9C09D7A9C807725DCEDB909050ECFF0F7F(__this, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebookGameObject::OnEnable()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebookGameObject_OnEnable_m15E6D53A86D9125635895AE188031CD4913E828B (AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidFacebookGameObject_OnSceneLoaded_mE8FDAADE1E19AA7202CB27DC8C033676B52700BD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SceneManager_tA0EF56A88ACA4A15731AF7FDC10A869FA4C698FA_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A* L_0 = (UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A*)il2cpp_codegen_object_new(UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		UnityAction_2__ctor_m0E0C01B7056EB1CB1E6C6F4FC457EBCA3F6B0041(L_0, __this, (intptr_t)((void*)AndroidFacebookGameObject_OnSceneLoaded_mE8FDAADE1E19AA7202CB27DC8C033676B52700BD_RuntimeMethod_var), NULL);
		il2cpp_codegen_runtime_class_init_inline(SceneManager_tA0EF56A88ACA4A15731AF7FDC10A869FA4C698FA_il2cpp_TypeInfo_var);
		SceneManager_add_sceneLoaded_m14BEBCC5E4A8DD2C806A48D79A4773315CB434C6(L_0, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebookGameObject::OnSceneLoaded(UnityEngine.SceneManagement.Scene,UnityEngine.SceneManagement.LoadSceneMode)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebookGameObject_OnSceneLoaded_mE8FDAADE1E19AA7202CB27DC8C033676B52700BD (AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* __this, Scene_tA1DC762B79745EB5140F054C884855B922318356 ___0_scene, int32_t ___1_mode, const RuntimeMethod* method) 
{
	{
		CodelessIAPAutoLog_addListenerToIAPButtons_mF59A2E9C09D7A9C807725DCEDB909050ECFF0F7F(__this, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebookGameObject::OnDisable()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebookGameObject_OnDisable_m32311E0E2E06960DE723D4A47BF7EC57889782F1 (AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidFacebookGameObject_OnSceneLoaded_mE8FDAADE1E19AA7202CB27DC8C033676B52700BD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SceneManager_tA0EF56A88ACA4A15731AF7FDC10A869FA4C698FA_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A* L_0 = (UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A*)il2cpp_codegen_object_new(UnityAction_2_t1C08AEB5AA4F72FEFAB7F303E33C8CFFF80A8C3A_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		UnityAction_2__ctor_m0E0C01B7056EB1CB1E6C6F4FC457EBCA3F6B0041(L_0, __this, (intptr_t)((void*)AndroidFacebookGameObject_OnSceneLoaded_mE8FDAADE1E19AA7202CB27DC8C033676B52700BD_RuntimeMethod_var), NULL);
		il2cpp_codegen_runtime_class_init_inline(SceneManager_tA0EF56A88ACA4A15731AF7FDC10A869FA4C698FA_il2cpp_TypeInfo_var);
		SceneManager_remove_sceneLoaded_m72A7C2A1B8EF1C21A208A9A015375577768B3978(L_0, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebookGameObject::onPurchaseCompleteHandler(System.Object)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebookGameObject_onPurchaseCompleteHandler_mE0D510C76A73484AF0B7C11E2DD7BD6D99B18EF7 (AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* __this, RuntimeObject* ___0_data, const RuntimeMethod* method) 
{
	{
		RuntimeObject* L_0 = ___0_data;
		CodelessIAPAutoLog_handlePurchaseCompleted_m06FA6E299411CFD7DB741AD50FBBC4F6F3E77CC4(L_0, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebookGameObject::OnLoginStatusRetrieved(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebookGameObject_OnLoginStatusRetrieved_m50EA4D72094D287AE88120ECD3906546AC8044FF (AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* __this, String_t* ___0_message, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0;
		L_0 = FacebookGameObject_get_Facebook_m8F6DC9F80E732D237D7F858FB1FC5D448071D137_inline(__this, NULL);
		String_t* L_1 = ___0_message;
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_2 = (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B*)il2cpp_codegen_object_new(ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B_il2cpp_TypeInfo_var);
		NullCheck(L_2);
		ResultContainer__ctor_mAD6B7ADD48D8244E67DEC442276642A1DB0EB74C(L_2, L_1, NULL);
		NullCheck(((AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*)CastclassSealed((RuntimeObject*)L_0, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD_il2cpp_TypeInfo_var)));
		AndroidFacebook_OnLoginStatusRetrieved_m65A558404816C7726AD1B59A65F8E35BD00A5F78(((AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*)CastclassSealed((RuntimeObject*)L_0, AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD_il2cpp_TypeInfo_var)), L_2, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebookGameObject::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebookGameObject__ctor_mD6C0FB2BD08E92351A31426D73C0AAC0914DFC86 (AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* __this, const RuntimeMethod* method) 
{
	{
		MobileFacebookGameObject__ctor_m21F7B347AB7787EBE63D37862A1BB5F191E9B17D(__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Facebook.Unity.FacebookGameObject Facebook.Unity.Mobile.Android.AndroidFacebookLoader::get_FBGameObject()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B* AndroidFacebookLoader_get_FBGameObject_m0066C68A46FA60BBD72042D9D2EA80E381CDD9AA (AndroidFacebookLoader_t581FB6217BD8B55FE1012FCF1AC8EDABE25320E9* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ComponentFactory_GetComponent_TisAndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378_mAE5A1A01D902A79AC16E3656B097F207B8C4C9EC_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* V_0 = NULL;
	{
		AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* L_0;
		L_0 = ComponentFactory_GetComponent_TisAndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378_mAE5A1A01D902A79AC16E3656B097F207B8C4C9EC(0, ComponentFactory_GetComponent_TisAndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378_mAE5A1A01D902A79AC16E3656B097F207B8C4C9EC_RuntimeMethod_var);
		V_0 = L_0;
		AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* L_1 = V_0;
		NullCheck(L_1);
		RuntimeObject* L_2;
		L_2 = FacebookGameObject_get_Facebook_m8F6DC9F80E732D237D7F858FB1FC5D448071D137_inline(L_1, NULL);
		if (L_2)
		{
			goto IL_001a;
		}
	}
	{
		AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* L_3 = V_0;
		AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* L_4 = (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD*)il2cpp_codegen_object_new(AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD_il2cpp_TypeInfo_var);
		NullCheck(L_4);
		AndroidFacebook__ctor_m4C9297CE471F88A30A971963689FAE8332984011(L_4, NULL);
		NullCheck(L_3);
		FacebookGameObject_set_Facebook_mEDCEAC5301992E16A962C3993F25C151F6251F1C_inline(L_3, L_4, NULL);
	}

IL_001a:
	{
		AndroidFacebookGameObject_tE6C1710A0A021C2A3758C18B550590F001B0E378* L_5 = V_0;
		return L_5;
	}
}
// System.Void Facebook.Unity.Mobile.Android.AndroidFacebookLoader::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidFacebookLoader__ctor_m7AD409122FD6048F992BA57F3BB0FC854D9C7343 (AndroidFacebookLoader_t581FB6217BD8B55FE1012FCF1AC8EDABE25320E9* __this, const RuntimeMethod* method) 
{
	{
		CompiledFacebookLoader__ctor_m0CEF02AE4B19FCB945491FD9DE8B4CAB0CB4F227(__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void Facebook.Unity.Canvas.CanvasFacebook::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook__ctor_m84A06718D125F7DEC19BD186F5D318F495248EFE (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0;
		L_0 = CanvasFacebook_GetCanvasJSWrapper_mCAF1873B89014661AA22463DB1FCF2FFA5FE3CFD(NULL);
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_1 = (CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749*)il2cpp_codegen_object_new(CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		CallbackManager__ctor_mBDDC9E4FCC6A9A0CCDE01755DC232BFCA66D4BA8(L_1, NULL);
		CanvasFacebook__ctor_m3ADD98C9D62A49DFCE51D16F6862FB80179E85E0(__this, L_0, L_1, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::.ctor(Facebook.Unity.Canvas.ICanvasJSWrapper,Facebook.Unity.CallbackManager)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook__ctor_m3ADD98C9D62A49DFCE51D16F6862FB80179E85E0 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, RuntimeObject* ___0_canvasJSWrapper, CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* ___1_callbackManager, const RuntimeMethod* method) 
{
	{
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_0 = ___1_callbackManager;
		FacebookBase__ctor_m91D39F3C0A029D203CF6A8E08546742B1D7A5AFA(__this, L_0, NULL);
		RuntimeObject* L_1 = ___0_canvasJSWrapper;
		__this->___canvasJSWrapper_5 = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___canvasJSWrapper_5), (void*)L_1);
		return;
	}
}
// Facebook.Unity.Canvas.ICanvasJSWrapper Facebook.Unity.Canvas.CanvasFacebook::GetCanvasJSWrapper()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* CanvasFacebook_GetCanvasJSWrapper_mCAF1873B89014661AA22463DB1FCF2FFA5FE3CFD (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral421262BE3B7342276C4DBB494A8258DF4A2AE9FD);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral88C9C0DAB7981E526880557EDF8F021E2F946C2D);
		s_Il2CppMethodInitialized = true;
	}
	{
		Assembly_t* L_0;
		L_0 = Assembly_Load_mC42733BACCA273EEAA32A341CBF53722A44DCC90(_stringLiteral88C9C0DAB7981E526880557EDF8F021E2F946C2D, NULL);
		NullCheck(L_0);
		Type_t* L_1;
		L_1 = VirtualFuncInvoker1< Type_t*, String_t* >::Invoke(19 /* System.Type System.Reflection.Assembly::GetType(System.String) */, L_0, _stringLiteral421262BE3B7342276C4DBB494A8258DF4A2AE9FD);
		RuntimeObject* L_2;
		L_2 = Activator_CreateInstance_mFF030428C64FDDFACC74DFAC97388A1C628BFBCF(L_1, NULL);
		return ((RuntimeObject*)Castclass((RuntimeObject*)L_2, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var));
	}
}
// System.Boolean Facebook.Unity.Canvas.CanvasFacebook::get_LimitEventUsage()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool CanvasFacebook_get_LimitEventUsage_m6E8EC596862C259390BFBA34758E526898FDEEB6 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, const RuntimeMethod* method) 
{
	{
		bool L_0 = __this->___U3CLimitEventUsageU3Ek__BackingField_7;
		return L_0;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::set_LimitEventUsage(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_set_LimitEventUsage_mE01AAF26FAC1854A4A6214ED5314C7E31B379587 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, bool ___0_value, const RuntimeMethod* method) 
{
	{
		bool L_0 = ___0_value;
		__this->___U3CLimitEventUsageU3Ek__BackingField_7 = L_0;
		return;
	}
}
// System.String Facebook.Unity.Canvas.CanvasFacebook::get_SDKName()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* CanvasFacebook_get_SDKName_mE46F50F1B61A9FE5A7B21610E33ABCE28F747ACF (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC329109D65F880A37C0127AB93EAA77E304327AB);
		s_Il2CppMethodInitialized = true;
	}
	{
		return _stringLiteralC329109D65F880A37C0127AB93EAA77E304327AB;
	}
}
// System.String Facebook.Unity.Canvas.CanvasFacebook::get_SDKVersion()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* CanvasFacebook_get_SDKVersion_m5AA4B8892A945B7791CCB3D86C00118E86A323DA (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___canvasJSWrapper_5;
		NullCheck(L_0);
		String_t* L_1;
		L_1 = InterfaceFuncInvoker0< String_t* >::Invoke(0 /* System.String Facebook.Unity.Canvas.ICanvasJSWrapper::GetSDKVersion() */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_0);
		return L_1;
	}
}
// System.String Facebook.Unity.Canvas.CanvasFacebook::get_SDKUserAgent()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* CanvasFacebook_get_SDKUserAgent_mFFE3C4E9F7435D98165C9CD52DCAC71CFEBDA8C1 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FacebookLogger_tAF941780684648D82AFA6B5D95965FAAFBBDB457_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FacebookUnityPlatform_tC5501D323BFD32B9D365B86D368A28653262A4D9_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1DF53E67AAE9B13FCBFF7748CBF5FD7DEA2088B6);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral8E752B76D455A50FE476984D4B09A7CDBF2A753E);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralDED9BB89C032BA6D29ED4E9E2D13C150B7C43B2E);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE0B03E56BE7EFC27DFD5B413B4A5DD74DC0FB1DF);
		s_Il2CppMethodInitialized = true;
	}
	String_t* V_0 = NULL;
	int32_t V_1 = 0;
	{
		int32_t L_0;
		L_0 = Constants_get_CurrentPlatform_mE6140F488BC2C82EC6FB0BE87184B6A8869050F8(NULL);
		if ((!(((uint32_t)L_0) == ((uint32_t)3))))
		{
			goto IL_0036;
		}
	}
	{
		il2cpp_codegen_runtime_class_init_inline(CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* L_1;
		L_1 = CultureInfo_get_InvariantCulture_mD1E96DC845E34B10F78CB744B0CB5D7D63CEB1E6(NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_3 = L_2;
		int32_t L_4;
		L_4 = Constants_get_CurrentPlatform_mE6140F488BC2C82EC6FB0BE87184B6A8869050F8(NULL);
		V_1 = L_4;
		Il2CppFakeBox<int32_t> L_5(FacebookUnityPlatform_tC5501D323BFD32B9D365B86D368A28653262A4D9_il2cpp_TypeInfo_var, (&V_1));
		String_t* L_6;
		L_6 = Enum_ToString_m946B0B83C4470457D0FF555D862022C72BB55741((Enum_t2A1A94B24E3B776EEF4E5E485E290BB9D4D072E2*)(&L_5), NULL);
		NullCheck(L_3);
		ArrayElementTypeCheck (L_3, L_6);
		(L_3)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_6);
		String_t* L_7;
		L_7 = String_Format_m447B585713E5EB3EBF5D9D0710706D01E8A56D75(L_1, _stringLiteral1DF53E67AAE9B13FCBFF7748CBF5FD7DEA2088B6, L_3, NULL);
		V_0 = L_7;
		goto IL_0046;
	}

IL_0036:
	{
		il2cpp_codegen_runtime_class_init_inline(FacebookLogger_tAF941780684648D82AFA6B5D95965FAAFBBDB457_il2cpp_TypeInfo_var);
		FacebookLogger_Warn_mDAD4A3F15FE52EF33DDC7A7B5639E04F057509D9(_stringLiteralE0B03E56BE7EFC27DFD5B413B4A5DD74DC0FB1DF, NULL);
		V_0 = _stringLiteralDED9BB89C032BA6D29ED4E9E2D13C150B7C43B2E;
	}

IL_0046:
	{
		il2cpp_codegen_runtime_class_init_inline(CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0_il2cpp_TypeInfo_var);
		CultureInfo_t9BA817D41AD55AC8BD07480DD8AC22F8FFA378E0* L_8;
		L_8 = CultureInfo_get_InvariantCulture_mD1E96DC845E34B10F78CB744B0CB5D7D63CEB1E6(NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_9 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)2);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_10 = L_9;
		String_t* L_11;
		L_11 = FacebookBase_get_SDKUserAgent_mDEA68FD4E4987F897BBAE09D4144B0159FEC408C(__this, NULL);
		NullCheck(L_10);
		ArrayElementTypeCheck (L_10, L_11);
		(L_10)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_11);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_12 = L_10;
		String_t* L_13 = V_0;
		String_t* L_14;
		L_14 = FacebookSdkVersion_get_Build_mBCD0ADAF8A500F19A9E9B15E820FB3C7726F598E(NULL);
		String_t* L_15;
		L_15 = Utilities_GetUserAgent_m338A5F74F5F5125C3D5D03DBC2A722CC0B8BC7D1(L_13, L_14, NULL);
		NullCheck(L_12);
		ArrayElementTypeCheck (L_12, L_15);
		(L_12)->SetAt(static_cast<il2cpp_array_size_t>(1), (RuntimeObject*)L_15);
		String_t* L_16;
		L_16 = String_Format_m447B585713E5EB3EBF5D9D0710706D01E8A56D75(L_8, _stringLiteral8E752B76D455A50FE476984D4B09A7CDBF2A753E, L_12, NULL);
		return L_16;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::Init(System.String,System.Boolean,System.Boolean,System.Boolean,System.Boolean,System.String,System.String,System.Boolean,System.String,System.Boolean,Facebook.Unity.HideUnityDelegate,Facebook.Unity.InitDelegate)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_Init_m9CB9B404293C16B33E4E1C4B7A937E0BA4896488 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, String_t* ___0_appId, bool ___1_cookie, bool ___2_logging, bool ___3_status, bool ___4_xfbml, String_t* ___5_channelUrl, String_t* ___6_authResponse, bool ___7_frictionlessRequests, String_t* ___8_javascriptSDKLocale, bool ___9_loadDebugJSSDK, HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE* ___10_hideUnityDelegate, InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F* ___11_onInitComplete, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FB_tD6AF917A642BEC6920761C8E4AD4013414829013_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral01C8B71057B916ADBDD8F28D026373D762E66FEF);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1696D3821CE92FE7EA9D4A8DA8A1CABCC6A3DA40);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2BEF6C0728DEA9AE2CE7B588B768DE3FC7583CC9);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral30F20681B7D1BDA035E4777AE6F8EF22F97224FC);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral44B679A4FEA54FC0DBB1CECD512FC3FCFFE445F4);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5A854D0C545A398D8E4AAB887ACBEDCC9D9BDF41);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralD2D2F8D3F9F04A081FFBE6B2AF7917BAAADFC052);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE8FC4145DFB9A299846498E30E00134EB0ED6753);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralEC79B3774CA39C0BF95A69EFBDEC79A32A5F3612);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralFD4706B02823C71252FBF63A74CF03433A8DADF0);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	String_t* G_B2_0 = NULL;
	String_t* G_B2_1 = NULL;
	RuntimeObject* G_B2_2 = NULL;
	String_t* G_B1_0 = NULL;
	String_t* G_B1_1 = NULL;
	RuntimeObject* G_B1_2 = NULL;
	int32_t G_B3_0 = 0;
	String_t* G_B3_1 = NULL;
	String_t* G_B3_2 = NULL;
	RuntimeObject* G_B3_3 = NULL;
	String_t* G_B5_0 = NULL;
	int32_t G_B5_1 = 0;
	String_t* G_B5_2 = NULL;
	String_t* G_B5_3 = NULL;
	RuntimeObject* G_B5_4 = NULL;
	String_t* G_B4_0 = NULL;
	int32_t G_B4_1 = 0;
	String_t* G_B4_2 = NULL;
	String_t* G_B4_3 = NULL;
	RuntimeObject* G_B4_4 = NULL;
	int32_t G_B6_0 = 0;
	String_t* G_B6_1 = NULL;
	int32_t G_B6_2 = 0;
	String_t* G_B6_3 = NULL;
	String_t* G_B6_4 = NULL;
	RuntimeObject* G_B6_5 = NULL;
	{
		InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F* L_0 = ___11_onInitComplete;
		FacebookBase_Init_m20FB2F18FDA2C3A4FA3EAD6C734EEFEAA5CF5F6A_inline(__this, L_0, NULL);
		RuntimeObject* L_1 = __this->___canvasJSWrapper_5;
		NullCheck(L_1);
		InterfaceActionInvoker0::Invoke(9 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::InitScreenPosition() */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_1);
		String_t* L_2 = ___0_appId;
		__this->___appId_3 = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___appId_3), (void*)L_2);
		HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE* L_3 = ___10_hideUnityDelegate;
		__this->___onHideUnityDelegate_6 = L_3;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___onHideUnityDelegate_6), (void*)L_3);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_4 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_4);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_4, NULL);
		V_0 = L_4;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		String_t* L_6 = ___0_appId;
		NullCheck(L_5);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_5, _stringLiteral30F20681B7D1BDA035E4777AE6F8EF22F97224FC, L_6, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_7 = V_0;
		bool L_8 = ___1_cookie;
		NullCheck(L_7);
		MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9(L_7, _stringLiteral44B679A4FEA54FC0DBB1CECD512FC3FCFFE445F4, L_8, MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_9 = V_0;
		bool L_10 = ___2_logging;
		NullCheck(L_9);
		MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9(L_9, _stringLiteralE8FC4145DFB9A299846498E30E00134EB0ED6753, L_10, MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_11 = V_0;
		bool L_12 = ___3_status;
		NullCheck(L_11);
		MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9(L_11, _stringLiteralFD4706B02823C71252FBF63A74CF03433A8DADF0, L_12, MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_13 = V_0;
		bool L_14 = ___4_xfbml;
		NullCheck(L_13);
		MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9(L_13, _stringLiteral5A854D0C545A398D8E4AAB887ACBEDCC9D9BDF41, L_14, MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_15 = V_0;
		String_t* L_16 = ___5_channelUrl;
		NullCheck(L_15);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_15, _stringLiteral1696D3821CE92FE7EA9D4A8DA8A1CABCC6A3DA40, L_16, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_17 = V_0;
		String_t* L_18 = ___6_authResponse;
		NullCheck(L_17);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_17, _stringLiteral2BEF6C0728DEA9AE2CE7B588B768DE3FC7583CC9, L_18, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_19 = V_0;
		bool L_20 = ___7_frictionlessRequests;
		NullCheck(L_19);
		MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9(L_19, _stringLiteral01C8B71057B916ADBDD8F28D026373D762E66FEF, L_20, MethodArguments_AddPrimative_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_mF6AB6D2AF667332865B6B52BBCBE202628C6A6E9_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_21 = V_0;
		il2cpp_codegen_runtime_class_init_inline(FB_tD6AF917A642BEC6920761C8E4AD4013414829013_il2cpp_TypeInfo_var);
		String_t* L_22;
		L_22 = FB_get_GraphApiVersion_m234E4C9258DE0F012E1A78D5A4DED1DE8D6D5198_inline(NULL);
		NullCheck(L_21);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_21, _stringLiteralD2D2F8D3F9F04A081FFBE6B2AF7917BAAADFC052, L_22, NULL);
		RuntimeObject* L_23 = __this->___canvasJSWrapper_5;
		String_t* L_24 = ___8_javascriptSDKLocale;
		bool L_25 = ___9_loadDebugJSSDK;
		G_B1_0 = L_24;
		G_B1_1 = _stringLiteralEC79B3774CA39C0BF95A69EFBDEC79A32A5F3612;
		G_B1_2 = L_23;
		if (L_25)
		{
			G_B2_0 = L_24;
			G_B2_1 = _stringLiteralEC79B3774CA39C0BF95A69EFBDEC79A32A5F3612;
			G_B2_2 = L_23;
			goto IL_00b1;
		}
	}
	{
		G_B3_0 = 0;
		G_B3_1 = G_B1_0;
		G_B3_2 = G_B1_1;
		G_B3_3 = G_B1_2;
		goto IL_00b2;
	}

IL_00b1:
	{
		G_B3_0 = 1;
		G_B3_1 = G_B2_0;
		G_B3_2 = G_B2_1;
		G_B3_3 = G_B2_2;
	}

IL_00b2:
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_26 = V_0;
		NullCheck(L_26);
		String_t* L_27;
		L_27 = MethodArguments_ToJsonString_m806D99FEC9348537F888D9D2DA012720952F9CF7(L_26, NULL);
		bool L_28 = ___3_status;
		G_B4_0 = L_27;
		G_B4_1 = G_B3_0;
		G_B4_2 = G_B3_1;
		G_B4_3 = G_B3_2;
		G_B4_4 = G_B3_3;
		if (L_28)
		{
			G_B5_0 = L_27;
			G_B5_1 = G_B3_0;
			G_B5_2 = G_B3_1;
			G_B5_3 = G_B3_2;
			G_B5_4 = G_B3_3;
			goto IL_00bf;
		}
	}
	{
		G_B6_0 = 0;
		G_B6_1 = G_B4_0;
		G_B6_2 = G_B4_1;
		G_B6_3 = G_B4_2;
		G_B6_4 = G_B4_3;
		G_B6_5 = G_B4_4;
		goto IL_00c0;
	}

IL_00bf:
	{
		G_B6_0 = 1;
		G_B6_1 = G_B5_0;
		G_B6_2 = G_B5_1;
		G_B6_3 = G_B5_2;
		G_B6_4 = G_B5_3;
		G_B6_5 = G_B5_4;
	}

IL_00c0:
	{
		NullCheck(G_B6_5);
		InterfaceActionInvoker5< String_t*, String_t*, int32_t, String_t*, int32_t >::Invoke(2 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::Init(System.String,System.String,System.Int32,System.String,System.Int32) */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, G_B6_5, G_B6_4, G_B6_3, G_B6_2, G_B6_1, G_B6_0);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::LogInWithPublishPermissions(System.Collections.Generic.IEnumerable`1<System.String>,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ILoginResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_LogInWithPublishPermissions_mDD1C186ED08556FCFF248E3E0319163453E01A28 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, RuntimeObject* ___0_permissions, FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CallbackManager_AddFacebookDelegate_TisILoginResult_t809F9817555CF3FF1F2C11154D110DB3F55C07ED_m8D256EDC847AB71366C71A4DA1FA1B3A51E782AC_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___canvasJSWrapper_5;
		NullCheck(L_0);
		InterfaceActionInvoker0::Invoke(1 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::DisableFullScreen() */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_0);
		RuntimeObject* L_1 = __this->___canvasJSWrapper_5;
		RuntimeObject* L_2 = ___0_permissions;
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_3;
		L_3 = FacebookBase_get_CallbackManager_m6C4BEBEF920CD139CF777D45E0E924829E4CF57F_inline(__this, NULL);
		FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* L_4 = ___1_callback;
		NullCheck(L_3);
		String_t* L_5;
		L_5 = CallbackManager_AddFacebookDelegate_TisILoginResult_t809F9817555CF3FF1F2C11154D110DB3F55C07ED_m8D256EDC847AB71366C71A4DA1FA1B3A51E782AC(L_3, L_4, CallbackManager_AddFacebookDelegate_TisILoginResult_t809F9817555CF3FF1F2C11154D110DB3F55C07ED_m8D256EDC847AB71366C71A4DA1FA1B3A51E782AC_RuntimeMethod_var);
		NullCheck(L_1);
		InterfaceActionInvoker2< RuntimeObject*, String_t* >::Invoke(3 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::Login(System.Collections.Generic.IEnumerable`1<System.String>,System.String) */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_1, L_2, L_5);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::LogInWithReadPermissions(System.Collections.Generic.IEnumerable`1<System.String>,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.ILoginResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_LogInWithReadPermissions_mE38B763C71C23F4A42092994AB11B0C29574EBA4 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, RuntimeObject* ___0_permissions, FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CallbackManager_AddFacebookDelegate_TisILoginResult_t809F9817555CF3FF1F2C11154D110DB3F55C07ED_m8D256EDC847AB71366C71A4DA1FA1B3A51E782AC_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___canvasJSWrapper_5;
		NullCheck(L_0);
		InterfaceActionInvoker0::Invoke(1 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::DisableFullScreen() */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_0);
		RuntimeObject* L_1 = __this->___canvasJSWrapper_5;
		RuntimeObject* L_2 = ___0_permissions;
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_3;
		L_3 = FacebookBase_get_CallbackManager_m6C4BEBEF920CD139CF777D45E0E924829E4CF57F_inline(__this, NULL);
		FacebookDelegate_1_t12EB4CA46E5479FC254BB457E8695ACBA6D7AB0D* L_4 = ___1_callback;
		NullCheck(L_3);
		String_t* L_5;
		L_5 = CallbackManager_AddFacebookDelegate_TisILoginResult_t809F9817555CF3FF1F2C11154D110DB3F55C07ED_m8D256EDC847AB71366C71A4DA1FA1B3A51E782AC(L_3, L_4, CallbackManager_AddFacebookDelegate_TisILoginResult_t809F9817555CF3FF1F2C11154D110DB3F55C07ED_m8D256EDC847AB71366C71A4DA1FA1B3A51E782AC_RuntimeMethod_var);
		NullCheck(L_1);
		InterfaceActionInvoker2< RuntimeObject*, String_t* >::Invoke(3 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::Login(System.Collections.Generic.IEnumerable`1<System.String>,System.String) */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_1, L_2, L_5);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::LogOut()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_LogOut_m80C55010FC178EE2C777DE9427FB1B35862CAE56 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		FacebookBase_LogOut_m2FFCD340816DE9D0B0AD32D5DDF89DFED7C62751(__this, NULL);
		RuntimeObject* L_0 = __this->___canvasJSWrapper_5;
		NullCheck(L_0);
		InterfaceActionInvoker0::Invoke(4 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::Logout() */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_0);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::AppRequest(System.String,System.Nullable`1<Facebook.Unity.OGActionType>,System.String,System.Collections.Generic.IEnumerable`1<System.String>,System.Collections.Generic.IEnumerable`1<System.Object>,System.Collections.Generic.IEnumerable`1<System.String>,System.Nullable`1<System.Int32>,System.String,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppRequestResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_AppRequest_m7EB4F779548999C3940633F11A4BE3BCE7317740 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, String_t* ___0_message, Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73 ___1_actionType, String_t* ___2_objectId, RuntimeObject* ___3_to, RuntimeObject* ___4_filters, RuntimeObject* ___5_excludeIds, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___6_maxRecipients, String_t* ___7_data, String_t* ___8_title, FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2* ___9_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasUIMethodCall_1__ctor_m3260CC061DE681C15F9551F21DBB0AB1914AD08D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_AddList_TisRuntimeObject_mD72031331AC5437B40C6D0570A060B9271CFF369_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_AddList_TisString_t_mBBEF6565354D6AE11E537FC132BC332546C22099_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m097F0A862D0980C829D48D394888D0370C9EFB85_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Nullable_1_ToString_mF981686677572249978468566375A4C296C6B97A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Nullable_1_get_HasValue_m3C0F9BCB83ED49443257921B53C3AC3A95FEDC63_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1C9D772538D2CCB196770E5E5AB06FBE4FBFD01F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral415902D5F52543D5ED4C8B796FB49950BFA51EF0);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4BDCC8C1F6304193EA13F4AFB26677B5B8AF161A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5E9D981D162913B37E33D18FF771488A4A34346E);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral98122CFEAFCE6941242F29CB3B619FAA1E78B828);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA4419EF51FB63A77978E414E01AC1C9DCF20AA99);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA493F2CAADD118807F8C20843D135AFAFCBBA254);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC7B4D926EF9532A71B25AEC040A33D52C926425F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralCB1BF9C3818A300118617C45409E803B828F1E9A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralD559C6D97E819D8E4EF7ACDC34C4E8D3DD314964);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	String_t* G_B2_0 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B2_1 = NULL;
	String_t* G_B1_0 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B1_1 = NULL;
	String_t* G_B3_0 = NULL;
	String_t* G_B3_1 = NULL;
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* G_B3_2 = NULL;
	{
		String_t* L_0 = ___0_message;
		Nullable_1_t61CE348507A9F5421A2CFD17C087DBF01CE99C73 L_1 = ___1_actionType;
		String_t* L_2 = ___2_objectId;
		RuntimeObject* L_3 = ___3_to;
		RuntimeObject* L_4 = ___4_filters;
		RuntimeObject* L_5 = ___5_excludeIds;
		Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 L_6 = ___6_maxRecipients;
		String_t* L_7 = ___7_data;
		String_t* L_8 = ___8_title;
		FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2* L_9 = ___9_callback;
		FacebookBase_ValidateAppRequestArgs_m90BA81706354BF100FBBCEDAD59FA567395266D8(__this, L_0, L_1, L_2, L_3, L_4, L_5, L_6, L_7, L_8, L_9, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_10 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_10);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_10, NULL);
		V_0 = L_10;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_11 = V_0;
		String_t* L_12 = ___0_message;
		NullCheck(L_11);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_11, _stringLiteralD559C6D97E819D8E4EF7ACDC34C4E8D3DD314964, L_12, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_13 = V_0;
		RuntimeObject* L_14 = ___3_to;
		NullCheck(L_13);
		MethodArguments_AddCommaSeparatedList_m4E3E59B109499EC3D6810CBEE16437DB193E0C38(L_13, _stringLiteralA4419EF51FB63A77978E414E01AC1C9DCF20AA99, L_14, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_15 = V_0;
		bool L_16;
		L_16 = Nullable_1_get_HasValue_m3C0F9BCB83ED49443257921B53C3AC3A95FEDC63_inline((&___1_actionType), Nullable_1_get_HasValue_m3C0F9BCB83ED49443257921B53C3AC3A95FEDC63_RuntimeMethod_var);
		G_B1_0 = _stringLiteral415902D5F52543D5ED4C8B796FB49950BFA51EF0;
		G_B1_1 = L_15;
		if (L_16)
		{
			G_B2_0 = _stringLiteral415902D5F52543D5ED4C8B796FB49950BFA51EF0;
			G_B2_1 = L_15;
			goto IL_0048;
		}
	}
	{
		G_B3_0 = ((String_t*)(NULL));
		G_B3_1 = G_B1_0;
		G_B3_2 = G_B1_1;
		goto IL_0055;
	}

IL_0048:
	{
		String_t* L_17;
		L_17 = Nullable_1_ToString_mF981686677572249978468566375A4C296C6B97A((&___1_actionType), Nullable_1_ToString_mF981686677572249978468566375A4C296C6B97A_RuntimeMethod_var);
		G_B3_0 = L_17;
		G_B3_1 = G_B2_0;
		G_B3_2 = G_B2_1;
	}

IL_0055:
	{
		NullCheck(G_B3_2);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(G_B3_2, G_B3_1, G_B3_0, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_18 = V_0;
		String_t* L_19 = ___2_objectId;
		NullCheck(L_18);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_18, _stringLiteral5E9D981D162913B37E33D18FF771488A4A34346E, L_19, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_20 = V_0;
		RuntimeObject* L_21 = ___4_filters;
		NullCheck(L_20);
		MethodArguments_AddList_TisRuntimeObject_mD72031331AC5437B40C6D0570A060B9271CFF369(L_20, _stringLiteral4BDCC8C1F6304193EA13F4AFB26677B5B8AF161A, L_21, MethodArguments_AddList_TisRuntimeObject_mD72031331AC5437B40C6D0570A060B9271CFF369_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_22 = V_0;
		RuntimeObject* L_23 = ___5_excludeIds;
		NullCheck(L_22);
		MethodArguments_AddList_TisString_t_mBBEF6565354D6AE11E537FC132BC332546C22099(L_22, _stringLiteral1C9D772538D2CCB196770E5E5AB06FBE4FBFD01F, L_23, MethodArguments_AddList_TisString_t_mBBEF6565354D6AE11E537FC132BC332546C22099_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_24 = V_0;
		Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 L_25 = ___6_maxRecipients;
		NullCheck(L_24);
		MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A(L_24, _stringLiteralCB1BF9C3818A300118617C45409E803B828F1E9A, L_25, MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_26 = V_0;
		String_t* L_27 = ___7_data;
		NullCheck(L_26);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_26, _stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469, L_27, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_28 = V_0;
		String_t* L_29 = ___8_title;
		NullCheck(L_28);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_28, _stringLiteralC7B4D926EF9532A71B25AEC040A33D52C926425F, L_29, NULL);
		CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE* L_30 = (CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE*)il2cpp_codegen_object_new(CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE_il2cpp_TypeInfo_var);
		NullCheck(L_30);
		CanvasUIMethodCall_1__ctor_m3260CC061DE681C15F9551F21DBB0AB1914AD08D(L_30, __this, _stringLiteral98122CFEAFCE6941242F29CB3B619FAA1E78B828, _stringLiteralA493F2CAADD118807F8C20843D135AFAFCBBA254, CanvasUIMethodCall_1__ctor_m3260CC061DE681C15F9551F21DBB0AB1914AD08D_RuntimeMethod_var);
		CanvasUIMethodCall_1_tCB2E3CEF2ECFFED4E9E70EF624A1A2CE0BF161DE* L_31 = L_30;
		FacebookDelegate_1_t0FD54CEBA449E8491C6F3CC16DAEC9723FBDEFF2* L_32 = ___9_callback;
		NullCheck(L_31);
		MethodCall_1_set_Callback_m097F0A862D0980C829D48D394888D0370C9EFB85_inline(L_31, L_32, MethodCall_1_set_Callback_m097F0A862D0980C829D48D394888D0370C9EFB85_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_33 = V_0;
		NullCheck(L_31);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IAppRequestResult>::Call(Facebook.Unity.MethodArguments) */, L_31, L_33);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::ActivateApp(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_ActivateApp_mE8DA5EE23CA9C034E85F0562ACE7CAADA9553CE3 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, String_t* ___0_appId, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___canvasJSWrapper_5;
		NullCheck(L_0);
		InterfaceActionInvoker0::Invoke(5 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::ActivateApp() */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_0);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::ShareLink(System.Uri,System.String,System.String,System.Uri,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IShareResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_ShareLink_mC2F448B9212E4A29F58F3C98E3941A43BBB6617A (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___0_contentURL, String_t* ___1_contentTitle, String_t* ___2_contentDescription, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___3_photoURL, FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* ___4_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasUIMethodCall_1__ctor_m09ADE50A6A66328C007485BF17C322DE3149D2CD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral18F572556D6BF6632E36F7AEF2204E0BB1DCD4D3);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2B8E19EBECECBC918E301E45BEEE7CE08295EF89);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5E89DFB28C21F05C783FF6C52C1E49EC5D97B20C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralCE18B047107AA23D1AA9B2ED32D316148E02655F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralDC51EFBBF1B5A672591C59CEF66A77386C61BE68);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralEB534843932D1025EEE09575458F840C63DC1063);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_2 = ___0_contentURL;
		NullCheck(L_1);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_1, _stringLiteral18F572556D6BF6632E36F7AEF2204E0BB1DCD4D3, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		String_t* L_4 = ___1_contentTitle;
		NullCheck(L_3);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_3, _stringLiteralCE18B047107AA23D1AA9B2ED32D316148E02655F, L_4, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		String_t* L_6 = ___2_contentDescription;
		NullCheck(L_5);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_5, _stringLiteralEB534843932D1025EEE09575458F840C63DC1063, L_6, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_7 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_8 = ___3_photoURL;
		NullCheck(L_7);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_7, _stringLiteral5E89DFB28C21F05C783FF6C52C1E49EC5D97B20C, L_8, NULL);
		CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052* L_9 = (CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052*)il2cpp_codegen_object_new(CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052_il2cpp_TypeInfo_var);
		NullCheck(L_9);
		CanvasUIMethodCall_1__ctor_m09ADE50A6A66328C007485BF17C322DE3149D2CD(L_9, __this, _stringLiteral2B8E19EBECECBC918E301E45BEEE7CE08295EF89, _stringLiteralDC51EFBBF1B5A672591C59CEF66A77386C61BE68, CanvasUIMethodCall_1__ctor_m09ADE50A6A66328C007485BF17C322DE3149D2CD_RuntimeMethod_var);
		CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052* L_10 = L_9;
		FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* L_11 = ___4_callback;
		NullCheck(L_10);
		MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_inline(L_10, L_11, MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_12 = V_0;
		NullCheck(L_10);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IShareResult>::Call(Facebook.Unity.MethodArguments) */, L_10, L_12);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::FeedShare(System.String,System.Uri,System.String,System.String,System.String,System.Uri,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IShareResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_FeedShare_m5FCAC2758D64A984D8202F573EE4F385DE7984D0 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, String_t* ___0_toId, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___1_link, String_t* ___2_linkName, String_t* ___3_linkCaption, String_t* ___4_linkDescription, Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* ___5_picture, String_t* ___6_mediaSource, FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* ___7_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasUIMethodCall_1__ctor_m09ADE50A6A66328C007485BF17C322DE3149D2CD_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral18F572556D6BF6632E36F7AEF2204E0BB1DCD4D3);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2B8E19EBECECBC918E301E45BEEE7CE08295EF89);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5B63100E80DAFEE5BA4AF4EDCCB7370ED6550264);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5E89DFB28C21F05C783FF6C52C1E49EC5D97B20C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral66F9618FDA792CAB23AF2D7FFB50AB2D3E393DC5);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA4419EF51FB63A77978E414E01AC1C9DCF20AA99);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralCE18B047107AA23D1AA9B2ED32D316148E02655F);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralDC51EFBBF1B5A672591C59CEF66A77386C61BE68);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralEB534843932D1025EEE09575458F840C63DC1063);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_toId;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteralA4419EF51FB63A77978E414E01AC1C9DCF20AA99, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_4 = ___1_link;
		NullCheck(L_3);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_3, _stringLiteral18F572556D6BF6632E36F7AEF2204E0BB1DCD4D3, L_4, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		String_t* L_6 = ___2_linkName;
		NullCheck(L_5);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_5, _stringLiteralCE18B047107AA23D1AA9B2ED32D316148E02655F, L_6, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_7 = V_0;
		String_t* L_8 = ___3_linkCaption;
		NullCheck(L_7);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_7, _stringLiteral5B63100E80DAFEE5BA4AF4EDCCB7370ED6550264, L_8, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_9 = V_0;
		String_t* L_10 = ___4_linkDescription;
		NullCheck(L_9);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_9, _stringLiteralEB534843932D1025EEE09575458F840C63DC1063, L_10, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_11 = V_0;
		Uri_t1500A52B5F71A04F5D05C0852D0F2A0941842A0E* L_12 = ___5_picture;
		NullCheck(L_11);
		MethodArguments_AddUri_m7649E111FCAA9094D3962942AC0643EA055CD1E6(L_11, _stringLiteral5E89DFB28C21F05C783FF6C52C1E49EC5D97B20C, L_12, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_13 = V_0;
		String_t* L_14 = ___6_mediaSource;
		NullCheck(L_13);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_13, _stringLiteral66F9618FDA792CAB23AF2D7FFB50AB2D3E393DC5, L_14, NULL);
		CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052* L_15 = (CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052*)il2cpp_codegen_object_new(CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052_il2cpp_TypeInfo_var);
		NullCheck(L_15);
		CanvasUIMethodCall_1__ctor_m09ADE50A6A66328C007485BF17C322DE3149D2CD(L_15, __this, _stringLiteral2B8E19EBECECBC918E301E45BEEE7CE08295EF89, _stringLiteralDC51EFBBF1B5A672591C59CEF66A77386C61BE68, CanvasUIMethodCall_1__ctor_m09ADE50A6A66328C007485BF17C322DE3149D2CD_RuntimeMethod_var);
		CanvasUIMethodCall_1_tD4275D9D165200BFC64C2957A7C89087D0BB0052* L_16 = L_15;
		FacebookDelegate_1_t4C7ACE222D7D3FBEA79B4E2B46A1CD632E0EAD35* L_17 = ___7_callback;
		NullCheck(L_16);
		MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_inline(L_16, L_17, MethodCall_1_set_Callback_m4DFB56A5441E5932700C867C4F2DF13A0FABC1F8_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_18 = V_0;
		NullCheck(L_16);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IShareResult>::Call(Facebook.Unity.MethodArguments) */, L_16, L_18);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::Pay(System.String,System.String,System.Int32,System.Nullable`1<System.Int32>,System.Nullable`1<System.Int32>,System.String,System.String,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_Pay_m9EA829A520213664925AE20AB50D2C13EA75CC72 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, String_t* ___0_product, String_t* ___1_action, int32_t ___2_quantity, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___3_quantityMin, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___4_quantityMax, String_t* ___5_requestId, String_t* ___6_pricepointId, String_t* ___7_testCurrency, FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B* ___8_callback, const RuntimeMethod* method) 
{
	{
		String_t* L_0 = ___0_product;
		String_t* L_1 = ___1_action;
		int32_t L_2 = ___2_quantity;
		Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 L_3 = ___3_quantityMin;
		Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 L_4 = ___4_quantityMax;
		String_t* L_5 = ___5_requestId;
		String_t* L_6 = ___6_pricepointId;
		String_t* L_7 = ___7_testCurrency;
		FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B* L_8 = ___8_callback;
		CanvasFacebook_PayImpl_m6559E97A2444BFABD5E6B5D4B8D14FB5DD984069(__this, L_0, (String_t*)NULL, L_1, L_2, L_3, L_4, L_5, L_6, L_7, (String_t*)NULL, L_8, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::GetAppLink(Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IAppLinkResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_GetAppLink_mF958026CDE246DB093863010513EDAADD159CE27 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D* ___0_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AppLinkResult_t7B3A5BB1C71DF6D48BBC7B508AE365E8D4A79143_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Dictionary_2_Add_m5875DF2ACE933D734119C088B2E7C9C63F49B443_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Dictionary_2__ctor_mC4F3DF292BAD88F4BF193C49CD689FAEBC4570A9_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&String_t_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral0458562336F91AC3F0C3FE71A886E75CE5C8F84A);
		s_Il2CppMethodInitialized = true;
	}
	Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* V_0 = NULL;
	{
		Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* L_0 = (Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710*)il2cpp_codegen_object_new(Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		Dictionary_2__ctor_mC4F3DF292BAD88F4BF193C49CD689FAEBC4570A9(L_0, Dictionary_2__ctor_mC4F3DF292BAD88F4BF193C49CD689FAEBC4570A9_RuntimeMethod_var);
		Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* L_1 = L_0;
		String_t* L_2 = __this->___appLinkUrl_4;
		NullCheck(L_1);
		Dictionary_2_Add_m5875DF2ACE933D734119C088B2E7C9C63F49B443(L_1, _stringLiteral0458562336F91AC3F0C3FE71A886E75CE5C8F84A, L_2, Dictionary_2_Add_m5875DF2ACE933D734119C088B2E7C9C63F49B443_RuntimeMethod_var);
		V_0 = L_1;
		FacebookDelegate_1_t084616048CD926AFFFA1D0E903E2278104EDFF5D* L_3 = ___0_callback;
		Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* L_4 = V_0;
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_5 = (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B*)il2cpp_codegen_object_new(ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B_il2cpp_TypeInfo_var);
		NullCheck(L_5);
		ResultContainer__ctor_m83395A80E1E9600322982ECDA27159717BBF44E7(L_5, L_4, NULL);
		AppLinkResult_t7B3A5BB1C71DF6D48BBC7B508AE365E8D4A79143* L_6 = (AppLinkResult_t7B3A5BB1C71DF6D48BBC7B508AE365E8D4A79143*)il2cpp_codegen_object_new(AppLinkResult_t7B3A5BB1C71DF6D48BBC7B508AE365E8D4A79143_il2cpp_TypeInfo_var);
		NullCheck(L_6);
		AppLinkResult__ctor_mB1D124600541A7821BCE9FCB190FDABA5C46FF59(L_6, L_5, NULL);
		NullCheck(L_3);
		FacebookDelegate_1_Invoke_m82AD0F3529DD1BA0DC1E547A1AC3C84DD1BC83A5_inline(L_3, L_6, NULL);
		String_t* L_7 = ((String_t_StaticFields*)il2cpp_codegen_static_fields_for(String_t_il2cpp_TypeInfo_var))->___Empty_6;
		__this->___appLinkUrl_4 = L_7;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___appLinkUrl_4), (void*)L_7);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::AppEventsLogEvent(System.String,System.Nullable`1<System.Single>,System.Collections.Generic.Dictionary`2<System.String,System.Object>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_AppEventsLogEvent_m3E83E524321F0B649623A9B0FA3C8BDFB5DBE649 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, String_t* ___0_logEvent, Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75 ___1_valueToSum, Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* ___2_parameters, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Json_t691FA1D65828CDA57416D327F6CBA3C911ABCE1E_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___canvasJSWrapper_5;
		String_t* L_1 = ___0_logEvent;
		Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75 L_2 = ___1_valueToSum;
		Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* L_3 = ___2_parameters;
		il2cpp_codegen_runtime_class_init_inline(Json_t691FA1D65828CDA57416D327F6CBA3C911ABCE1E_il2cpp_TypeInfo_var);
		String_t* L_4;
		L_4 = Json_Serialize_mB946D73EA18C733AA78049E62EFD0D8BD04328CB(L_3, NULL);
		NullCheck(L_0);
		InterfaceActionInvoker3< String_t*, Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75, String_t* >::Invoke(6 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::LogAppEvent(System.String,System.Nullable`1<System.Single>,System.String) */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_0, L_1, L_2, L_4);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::AppEventsLogPurchase(System.Single,System.String,System.Collections.Generic.Dictionary`2<System.String,System.Object>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_AppEventsLogPurchase_mA9F046615C36B3AC163AFB32A7CCEBC09D0CB2FE (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, float ___0_purchaseAmount, String_t* ___1_currency, Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* ___2_parameters, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Json_t691FA1D65828CDA57416D327F6CBA3C911ABCE1E_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___canvasJSWrapper_5;
		float L_1 = ___0_purchaseAmount;
		String_t* L_2 = ___1_currency;
		Dictionary_2_tA348003A3C1CEFB3096E9D2A0BC7F1AC8EC4F710* L_3 = ___2_parameters;
		il2cpp_codegen_runtime_class_init_inline(Json_t691FA1D65828CDA57416D327F6CBA3C911ABCE1E_il2cpp_TypeInfo_var);
		String_t* L_4;
		L_4 = Json_Serialize_mB946D73EA18C733AA78049E62EFD0D8BD04328CB(L_3, NULL);
		NullCheck(L_0);
		InterfaceActionInvoker3< float, String_t*, String_t* >::Invoke(7 /* System.Void Facebook.Unity.Canvas.ICanvasJSWrapper::LogPurchase(System.Single,System.String,System.String) */, ICanvasJSWrapper_t1748EA8C60A8447BF5907D9F7EBEAF381AAA0B12_il2cpp_TypeInfo_var, L_0, L_1, L_2, L_4);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::OnLoginComplete(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_OnLoginComplete_m31B8BC35ACD40C5211D6C7A59DBF95FD049788A2 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_result, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasFacebook_U3COnLoginCompleteU3Eb__37_0_mECC453F16A4ABDD1C5301986A3265C98AA4F3945_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_0 = ___0_result;
		Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* L_1 = (Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92*)il2cpp_codegen_object_new(Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		Callback_1__ctor_mE4AFB946F865A019EC8E67BFE4552B5F7FC4DA51(L_1, __this, (intptr_t)((void*)CanvasFacebook_U3COnLoginCompleteU3Eb__37_0_mECC453F16A4ABDD1C5301986A3265C98AA4F3945_RuntimeMethod_var), NULL);
		CanvasFacebook_FormatAuthResponse_mF113AE6F0CECF177B251B4989E62E404E69BB0BF(L_0, L_1, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::OnGetAppLinkComplete(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_OnGetAppLinkComplete_mA941FB6E6FC4EE30B35515478C5C5CC4CA2E8F64 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_message, const RuntimeMethod* method) 
{
	{
		NotImplementedException_t6366FE4DCF15094C51F4833B91A2AE68D4DA90E8* L_0 = (NotImplementedException_t6366FE4DCF15094C51F4833B91A2AE68D4DA90E8*)il2cpp_codegen_object_new(((RuntimeClass*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&NotImplementedException_t6366FE4DCF15094C51F4833B91A2AE68D4DA90E8_il2cpp_TypeInfo_var)));
		NullCheck(L_0);
		NotImplementedException__ctor_mDAB47BC6BD0E342E8F2171E5CABE3E67EA049F1C(L_0, NULL);
		IL2CPP_RAISE_MANAGED_EXCEPTION(L_0, ((RuntimeMethod*)il2cpp_codegen_initialize_runtime_metadata_inline((uintptr_t*)&CanvasFacebook_OnGetAppLinkComplete_mA941FB6E6FC4EE30B35515478C5C5CC4CA2E8F64_RuntimeMethod_var)));
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::OnFacebookAuthResponseChange(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_OnFacebookAuthResponseChange_m007239F26735F40030DC4B8E887370CA1045CEF8 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_U3COnFacebookAuthResponseChangeU3Eb__40_0_m6BDA2E988B1B4F39B882D8A24DE94F228F1B481F_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* G_B2_0 = NULL;
	ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* G_B2_1 = NULL;
	Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* G_B1_0 = NULL;
	ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* G_B1_1 = NULL;
	{
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_0 = ___0_resultContainer;
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var);
		Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* L_1 = ((U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var))->___U3CU3E9__40_0_1;
		Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* L_2 = L_1;
		G_B1_0 = L_2;
		G_B1_1 = L_0;
		if (L_2)
		{
			G_B2_0 = L_2;
			G_B2_1 = L_0;
			goto IL_0020;
		}
	}
	{
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var);
		U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8* L_3 = ((U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var))->___U3CU3E9_0;
		Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* L_4 = (Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92*)il2cpp_codegen_object_new(Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92_il2cpp_TypeInfo_var);
		NullCheck(L_4);
		Callback_1__ctor_mE4AFB946F865A019EC8E67BFE4552B5F7FC4DA51(L_4, L_3, (intptr_t)((void*)U3CU3Ec_U3COnFacebookAuthResponseChangeU3Eb__40_0_m6BDA2E988B1B4F39B882D8A24DE94F228F1B481F_RuntimeMethod_var), NULL);
		Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* L_5 = L_4;
		((U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var))->___U3CU3E9__40_0_1 = L_5;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var))->___U3CU3E9__40_0_1), (void*)L_5);
		G_B2_0 = L_5;
		G_B2_1 = G_B1_1;
	}

IL_0020:
	{
		CanvasFacebook_FormatAuthResponse_mF113AE6F0CECF177B251B4989E62E404E69BB0BF(G_B2_1, G_B2_0, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::OnPayComplete(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_OnPayComplete_m92E1525EF1656737630DC7D82B5A6A58C73A6B38 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08* V_0 = NULL;
	{
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_0 = ___0_resultContainer;
		PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08* L_1 = (PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08*)il2cpp_codegen_object_new(PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		PayResult__ctor_mD2C8086EE7A0649161C6E217605A1AB933E86B95(L_1, L_0, NULL);
		V_0 = L_1;
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_2;
		L_2 = FacebookBase_get_CallbackManager_m6C4BEBEF920CD139CF777D45E0E924829E4CF57F_inline(__this, NULL);
		PayResult_t660CF4D34A9624F7C61E2E5CF2749129D480DE08* L_3 = V_0;
		NullCheck(L_2);
		CallbackManager_OnFacebookResponse_mA506CEDF0220B6EA8F5D36F639E1840F15E255A6(L_2, L_3, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::OnAppRequestsComplete(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_OnAppRequestsComplete_m88AAA34CC6141AF5E96370E9142880EB2A474EC1 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2* V_0 = NULL;
	{
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_0 = ___0_resultContainer;
		AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2* L_1 = (AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2*)il2cpp_codegen_object_new(AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		AppRequestResult__ctor_m4595618FC82A0143E3A8799D4252798FF0AD643D(L_1, L_0, NULL);
		V_0 = L_1;
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_2;
		L_2 = FacebookBase_get_CallbackManager_m6C4BEBEF920CD139CF777D45E0E924829E4CF57F_inline(__this, NULL);
		AppRequestResult_t243A4AE32AD282D83D84B2419689B8FD77DD73C2* L_3 = V_0;
		NullCheck(L_2);
		CallbackManager_OnFacebookResponse_mA506CEDF0220B6EA8F5D36F639E1840F15E255A6(L_2, L_3, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::OnShareLinkComplete(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_OnShareLinkComplete_m71D918C8B490675B09F8591AF900EB8A76C06E42 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_resultContainer, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0* V_0 = NULL;
	{
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_0 = ___0_resultContainer;
		ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0* L_1 = (ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0*)il2cpp_codegen_object_new(ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		ShareResult__ctor_m5AAAC8595A4E1839AEDE81113879546BBE11458F(L_1, L_0, NULL);
		V_0 = L_1;
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_2;
		L_2 = FacebookBase_get_CallbackManager_m6C4BEBEF920CD139CF777D45E0E924829E4CF57F_inline(__this, NULL);
		ShareResult_tB593A0F8CCC1A4DA05B0685BE227C7CEC79FB8D0* L_3 = V_0;
		NullCheck(L_2);
		CallbackManager_OnFacebookResponse_mA506CEDF0220B6EA8F5D36F639E1840F15E255A6(L_2, L_3, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::OnUrlResponse(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_OnUrlResponse_m8C2FEFF33E69C66ACDECD490D243A1E0830FAA21 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, String_t* ___0_url, const RuntimeMethod* method) 
{
	{
		String_t* L_0 = ___0_url;
		__this->___appLinkUrl_4 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___appLinkUrl_4), (void*)L_0);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::OnHideUnity(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_OnHideUnity_m2163ADB8F4B3014E79C08D53B099B9A80A994D03 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, bool ___0_isGameShown, const RuntimeMethod* method) 
{
	{
		HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE* L_0 = __this->___onHideUnityDelegate_6;
		if (!L_0)
		{
			goto IL_0014;
		}
	}
	{
		HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE* L_1 = __this->___onHideUnityDelegate_6;
		bool L_2 = ___0_isGameShown;
		NullCheck(L_1);
		HideUnityDelegate_Invoke_m84D79F9CD775257293DF8E35297F43835E18CD71_inline(L_1, L_2, NULL);
	}

IL_0014:
	{
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::FormatAuthResponse(Facebook.Unity.ResultContainer,Facebook.Unity.Utilities/Callback`1<Facebook.Unity.ResultContainer>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_FormatAuthResponse_mF113AE6F0CECF177B251B4989E62E404E69BB0BF (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_result, Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* ___1_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Dictionary_2_Add_mC78C20D5901C87AAC38F37C906FAB6946BDE5F13_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Dictionary_2__ctor_m768E076F1E804CE4959F4E71D3E6A9ADE2F55052_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FB_tD6AF917A642BEC6920761C8E4AD4013414829013_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IDisposable_t030E0496B4E0E4E4F086825007979AF51F7248C5_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IEnumerable_1_t1F32F711C91AEBCFA4770668CA067447D2A4F665_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IEnumerator_1_t913F242211877D391217C9D75152235266FDAF10_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IEnumerator_t7B609C2FFA6EB5167D9C62A0C32A21DE2F666DAA_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&KeyValuePair_2_get_Key_mA64FF29A08423140758B0276333D1A89C71B793A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&KeyValuePair_2_get_Value_m2052BF44A3FDE623D98B0E6B6E227B2900034235_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&String_t_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec__DisplayClass47_0_U3CFormatAuthResponseU3Eb__0_m601241C800917FCB891C39BCCEA9988624FDE94A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Utilities_TryGetValue_TisIDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_mF4F1612A6CC71E09B0A940E7B840C94637D8BB97_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1405C2A661574468F6107DE8ADDF274A347D4F54);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2BEF6C0728DEA9AE2CE7B588B768DE3FC7583CC9);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5D0B7A1BC952F7F3CBB1B00E89E84090A2286942);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral85169FA7FA3BE7DC7C81A7D284020DF267C61183);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE79F08E3990626B5F3144E992D4D2F7D14584EC0);
		s_Il2CppMethodInitialized = true;
	}
	U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* V_0 = NULL;
	RuntimeObject* V_1 = NULL;
	RuntimeObject* V_2 = NULL;
	KeyValuePair_2_tBEE55F2A4574C64393155C322376FD98C7BFC7B9 V_3;
	memset((&V_3), 0, sizeof(V_3));
	Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* V_4 = NULL;
	FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB* V_5 = NULL;
	{
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_0 = (U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A*)il2cpp_codegen_object_new(U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		U3CU3Ec__DisplayClass47_0__ctor_m8D4983F5B36F90967DDFDD0051DBDD7BE6E39DE7(L_0, NULL);
		V_0 = L_0;
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_1 = V_0;
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_2 = ___0_result;
		NullCheck(L_1);
		L_1->___result_0 = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&L_1->___result_0), (void*)L_2);
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_3 = V_0;
		Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* L_4 = ___1_callback;
		NullCheck(L_3);
		L_3->___callback_1 = L_4;
		Il2CppCodeGenWriteBarrier((void**)(&L_3->___callback_1), (void*)L_4);
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_5 = V_0;
		NullCheck(L_5);
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_6 = L_5->___result_0;
		NullCheck(L_6);
		RuntimeObject* L_7;
		L_7 = ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline(L_6, NULL);
		if (L_7)
		{
			goto IL_0033;
		}
	}
	{
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_8 = V_0;
		NullCheck(L_8);
		Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* L_9 = L_8->___callback_1;
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_10 = V_0;
		NullCheck(L_10);
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_11 = L_10->___result_0;
		NullCheck(L_9);
		Callback_1_Invoke_mFD8528526031568E661F81643EF709689D3884B0_inline(L_9, L_11, NULL);
		return;
	}

IL_0033:
	{
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_12 = V_0;
		NullCheck(L_12);
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_13 = L_12->___result_0;
		NullCheck(L_13);
		RuntimeObject* L_14;
		L_14 = ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline(L_13, NULL);
		bool L_15;
		L_15 = Utilities_TryGetValue_TisIDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_mF4F1612A6CC71E09B0A940E7B840C94637D8BB97(L_14, _stringLiteral2BEF6C0728DEA9AE2CE7B588B768DE3FC7583CC9, (&V_1), Utilities_TryGetValue_TisIDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_mF4F1612A6CC71E09B0A940E7B840C94637D8BB97_RuntimeMethod_var);
		if (!L_15)
		{
			goto IL_00a4;
		}
	}
	{
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_16 = V_0;
		NullCheck(L_16);
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_17 = L_16->___result_0;
		NullCheck(L_17);
		RuntimeObject* L_18;
		L_18 = ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline(L_17, NULL);
		NullCheck(L_18);
		bool L_19;
		L_19 = InterfaceFuncInvoker1< bool, String_t* >::Invoke(6 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.Object>::Remove(TKey) */, IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var, L_18, _stringLiteral2BEF6C0728DEA9AE2CE7B588B768DE3FC7583CC9);
		RuntimeObject* L_20 = V_1;
		NullCheck(L_20);
		RuntimeObject* L_21;
		L_21 = InterfaceFuncInvoker0< RuntimeObject* >::Invoke(0 /* System.Collections.Generic.IEnumerator`1<T> System.Collections.Generic.IEnumerable`1<System.Collections.Generic.KeyValuePair`2<System.String,System.Object>>::GetEnumerator() */, IEnumerable_1_t1F32F711C91AEBCFA4770668CA067447D2A4F665_il2cpp_TypeInfo_var, L_20);
		V_2 = L_21;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_009a:
			{// begin finally (depth: 1)
				{
					RuntimeObject* L_22 = V_2;
					if (!L_22)
					{
						goto IL_00a3;
					}
				}
				{
					RuntimeObject* L_23 = V_2;
					NullCheck(L_23);
					InterfaceActionInvoker0::Invoke(0 /* System.Void System.IDisposable::Dispose() */, IDisposable_t030E0496B4E0E4E4F086825007979AF51F7248C5_il2cpp_TypeInfo_var, L_23);
				}

IL_00a3:
				{
					return;
				}
			}// end finally (depth: 1)
		});
		try
		{// begin try (depth: 1)
			{
				goto IL_0090_1;
			}

IL_006b_1:
			{
				RuntimeObject* L_24 = V_2;
				NullCheck(L_24);
				KeyValuePair_2_tBEE55F2A4574C64393155C322376FD98C7BFC7B9 L_25;
				L_25 = InterfaceFuncInvoker0< KeyValuePair_2_tBEE55F2A4574C64393155C322376FD98C7BFC7B9 >::Invoke(0 /* T System.Collections.Generic.IEnumerator`1<System.Collections.Generic.KeyValuePair`2<System.String,System.Object>>::get_Current() */, IEnumerator_1_t913F242211877D391217C9D75152235266FDAF10_il2cpp_TypeInfo_var, L_24);
				V_3 = L_25;
				U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_26 = V_0;
				NullCheck(L_26);
				ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_27 = L_26->___result_0;
				NullCheck(L_27);
				RuntimeObject* L_28;
				L_28 = ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline(L_27, NULL);
				String_t* L_29;
				L_29 = KeyValuePair_2_get_Key_mA64FF29A08423140758B0276333D1A89C71B793A_inline((&V_3), KeyValuePair_2_get_Key_mA64FF29A08423140758B0276333D1A89C71B793A_RuntimeMethod_var);
				RuntimeObject* L_30;
				L_30 = KeyValuePair_2_get_Value_m2052BF44A3FDE623D98B0E6B6E227B2900034235_inline((&V_3), KeyValuePair_2_get_Value_m2052BF44A3FDE623D98B0E6B6E227B2900034235_RuntimeMethod_var);
				NullCheck(L_28);
				InterfaceActionInvoker2< String_t*, RuntimeObject* >::Invoke(1 /* System.Void System.Collections.Generic.IDictionary`2<System.String,System.Object>::set_Item(TKey,TValue) */, IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var, L_28, L_29, L_30);
			}

IL_0090_1:
			{
				RuntimeObject* L_31 = V_2;
				NullCheck(L_31);
				bool L_32;
				L_32 = InterfaceFuncInvoker0< bool >::Invoke(0 /* System.Boolean System.Collections.IEnumerator::MoveNext() */, IEnumerator_t7B609C2FFA6EB5167D9C62A0C32A21DE2F666DAA_il2cpp_TypeInfo_var, L_31);
				if (L_32)
				{
					goto IL_006b_1;
				}
			}
			{
				goto IL_00a4;
			}
		}// end try (depth: 1)
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_00a4:
	{
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_33 = V_0;
		NullCheck(L_33);
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_34 = L_33->___result_0;
		NullCheck(L_34);
		RuntimeObject* L_35;
		L_35 = ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline(L_34, NULL);
		il2cpp_codegen_runtime_class_init_inline(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		String_t* L_36 = ((LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_StaticFields*)il2cpp_codegen_static_fields_for(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var))->___AccessTokenKey_10;
		NullCheck(L_35);
		bool L_37;
		L_37 = InterfaceFuncInvoker1< bool, String_t* >::Invoke(4 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.Object>::ContainsKey(TKey) */, IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var, L_35, L_36);
		if (!L_37)
		{
			goto IL_012c;
		}
	}
	{
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_38 = V_0;
		NullCheck(L_38);
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_39 = L_38->___result_0;
		NullCheck(L_39);
		RuntimeObject* L_40;
		L_40 = ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline(L_39, NULL);
		il2cpp_codegen_runtime_class_init_inline(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		String_t* L_41 = ((LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_StaticFields*)il2cpp_codegen_static_fields_for(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var))->___PermissionsKey_9;
		NullCheck(L_40);
		bool L_42;
		L_42 = InterfaceFuncInvoker1< bool, String_t* >::Invoke(4 /* System.Boolean System.Collections.Generic.IDictionary`2<System.String,System.Object>::ContainsKey(TKey) */, IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var, L_40, L_41);
		if (L_42)
		{
			goto IL_012c;
		}
	}
	{
		Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* L_43 = (Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83*)il2cpp_codegen_object_new(Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83_il2cpp_TypeInfo_var);
		NullCheck(L_43);
		Dictionary_2__ctor_m768E076F1E804CE4959F4E71D3E6A9ADE2F55052(L_43, Dictionary_2__ctor_m768E076F1E804CE4959F4E71D3E6A9ADE2F55052_RuntimeMethod_var);
		Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* L_44 = L_43;
		NullCheck(L_44);
		Dictionary_2_Add_mC78C20D5901C87AAC38F37C906FAB6946BDE5F13(L_44, _stringLiteral85169FA7FA3BE7DC7C81A7D284020DF267C61183, _stringLiteralE79F08E3990626B5F3144E992D4D2F7D14584EC0, Dictionary_2_Add_mC78C20D5901C87AAC38F37C906FAB6946BDE5F13_RuntimeMethod_var);
		Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* L_45 = L_44;
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_46 = V_0;
		NullCheck(L_46);
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_47 = L_46->___result_0;
		NullCheck(L_47);
		RuntimeObject* L_48;
		L_48 = ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline(L_47, NULL);
		il2cpp_codegen_runtime_class_init_inline(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		String_t* L_49 = ((LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_StaticFields*)il2cpp_codegen_static_fields_for(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var))->___AccessTokenKey_10;
		NullCheck(L_48);
		RuntimeObject* L_50;
		L_50 = InterfaceFuncInvoker1< RuntimeObject*, String_t* >::Invoke(0 /* TValue System.Collections.Generic.IDictionary`2<System.String,System.Object>::get_Item(TKey) */, IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var, L_48, L_49);
		NullCheck(L_45);
		Dictionary_2_Add_mC78C20D5901C87AAC38F37C906FAB6946BDE5F13(L_45, _stringLiteral1405C2A661574468F6107DE8ADDF274A347D4F54, ((String_t*)CastclassSealed((RuntimeObject*)L_50, String_t_il2cpp_TypeInfo_var)), Dictionary_2_Add_mC78C20D5901C87AAC38F37C906FAB6946BDE5F13_RuntimeMethod_var);
		V_4 = L_45;
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_51 = V_0;
		FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB* L_52 = (FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB*)il2cpp_codegen_object_new(FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB_il2cpp_TypeInfo_var);
		NullCheck(L_52);
		FacebookDelegate_1__ctor_m9A4C43BB75B9BA8CAE0A29F54DFFC9F4A7EE1F27(L_52, L_51, (intptr_t)((void*)U3CU3Ec__DisplayClass47_0_U3CFormatAuthResponseU3Eb__0_m601241C800917FCB891C39BCCEA9988624FDE94A_RuntimeMethod_var), NULL);
		V_5 = L_52;
		FacebookDelegate_1_t3F4605F93830396F9B98FFF45A1DF358D8D1C6FB* L_53 = V_5;
		Dictionary_2_t46B2DB028096FA2B828359E52F37F3105A83AD83* L_54 = V_4;
		il2cpp_codegen_runtime_class_init_inline(FB_tD6AF917A642BEC6920761C8E4AD4013414829013_il2cpp_TypeInfo_var);
		FB_API_m99EA09DCB400B85BC92B3E9E887D2D934B999B14(_stringLiteral5D0B7A1BC952F7F3CBB1B00E89E84090A2286942, 0, L_53, L_54, NULL);
		return;
	}

IL_012c:
	{
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_55 = V_0;
		NullCheck(L_55);
		Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* L_56 = L_55->___callback_1;
		U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* L_57 = V_0;
		NullCheck(L_57);
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_58 = L_57->___result_0;
		NullCheck(L_56);
		Callback_1_Invoke_mFD8528526031568E661F81643EF709689D3884B0_inline(L_56, L_58, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::PayImpl(System.String,System.String,System.String,System.Int32,System.Nullable`1<System.Int32>,System.Nullable`1<System.Int32>,System.String,System.String,System.String,System.String,Facebook.Unity.FacebookDelegate`1<Facebook.Unity.IPayResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_PayImpl_m6559E97A2444BFABD5E6B5D4B8D14FB5DD984069 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, String_t* ___0_product, String_t* ___1_productId, String_t* ___2_action, int32_t ___3_quantity, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___4_quantityMin, Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 ___5_quantityMax, String_t* ___6_requestId, String_t* ___7_pricepointId, String_t* ___8_testCurrency, String_t* ___9_developerPayload, FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B* ___10_callback, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasUIMethodCall_1__ctor_m1C9CF2D82C5B3A9F345A89045A43FF89270325F9_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&MethodCall_1_set_Callback_m2FE69B0E3B56B162D1F585BF08320F54F3EB73CB_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1630E6A6E4B065CB228F2BB0735FC4EB04ADCF98);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral1F751E5A1E5A1E5CE85E82DD29AF64763DD5DA62);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4502F3B52C9C8E53863433138E98E29E02FA04DB);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral49FDF777B468FCDB3266D04859340340232DD589);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5A54AF115B00E5E16EBAE4AA0D0BC51A13317E05);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral65DCC08617A31D5E06A9612DBABB862DA1C4F062);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA45D1B7DECAFE0936897477A36E2DEEBDF9C830A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralBC5DC870084E6ECC69F6896E17DF1CD0CC850F9A);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralDE0657B16868C29C06BE2B1767DAD49310029B58);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE3CFDB22842F54E3E0B119ACE3245306F0309BE0);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF51C0F9CE38D4A8832919DB2A9A19DC212BB790E);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF9010398F7F524C05AB19445BDCE02E617A3E267);
		s_Il2CppMethodInitialized = true;
	}
	MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* V_0 = NULL;
	{
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_0 = (MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD*)il2cpp_codegen_object_new(MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		MethodArguments__ctor_mF01989BE91F2F4509868A8937EE825C89D072974(L_0, NULL);
		V_0 = L_0;
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_1 = V_0;
		String_t* L_2 = ___0_product;
		NullCheck(L_1);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_1, _stringLiteral1630E6A6E4B065CB228F2BB0735FC4EB04ADCF98, L_2, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_3 = V_0;
		String_t* L_4 = ___1_productId;
		NullCheck(L_3);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_3, _stringLiteral1F751E5A1E5A1E5CE85E82DD29AF64763DD5DA62, L_4, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_5 = V_0;
		String_t* L_6 = ___2_action;
		NullCheck(L_5);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_5, _stringLiteralF9010398F7F524C05AB19445BDCE02E617A3E267, L_6, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_7 = V_0;
		int32_t L_8 = ___3_quantity;
		NullCheck(L_7);
		MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC(L_7, _stringLiteralF51C0F9CE38D4A8832919DB2A9A19DC212BB790E, L_8, MethodArguments_AddPrimative_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m0982983DAB9EEF94D7C05A9A8D6495709064C9CC_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_9 = V_0;
		Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 L_10 = ___4_quantityMin;
		NullCheck(L_9);
		MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A(L_9, _stringLiteralE3CFDB22842F54E3E0B119ACE3245306F0309BE0, L_10, MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_11 = V_0;
		Nullable_1_tCF32C56A2641879C053C86F273C0C6EC1B40BC28 L_12 = ___5_quantityMax;
		NullCheck(L_11);
		MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A(L_11, _stringLiteral49FDF777B468FCDB3266D04859340340232DD589, L_12, MethodArguments_AddNullablePrimitive_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m9492F4E106499E2D07719811BC5065CAC987632A_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_13 = V_0;
		String_t* L_14 = ___6_requestId;
		NullCheck(L_13);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_13, _stringLiteralBC5DC870084E6ECC69F6896E17DF1CD0CC850F9A, L_14, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_15 = V_0;
		String_t* L_16 = ___7_pricepointId;
		NullCheck(L_15);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_15, _stringLiteral5A54AF115B00E5E16EBAE4AA0D0BC51A13317E05, L_16, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_17 = V_0;
		String_t* L_18 = ___8_testCurrency;
		NullCheck(L_17);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_17, _stringLiteral4502F3B52C9C8E53863433138E98E29E02FA04DB, L_18, NULL);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_19 = V_0;
		String_t* L_20 = ___9_developerPayload;
		NullCheck(L_19);
		MethodArguments_AddString_m4772ADF5496756C596691E77474DC62DEDB983BC(L_19, _stringLiteral65DCC08617A31D5E06A9612DBABB862DA1C4F062, L_20, NULL);
		CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6* L_21 = (CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6*)il2cpp_codegen_object_new(CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6_il2cpp_TypeInfo_var);
		NullCheck(L_21);
		CanvasUIMethodCall_1__ctor_m1C9CF2D82C5B3A9F345A89045A43FF89270325F9(L_21, __this, _stringLiteralDE0657B16868C29C06BE2B1767DAD49310029B58, _stringLiteralA45D1B7DECAFE0936897477A36E2DEEBDF9C830A, CanvasUIMethodCall_1__ctor_m1C9CF2D82C5B3A9F345A89045A43FF89270325F9_RuntimeMethod_var);
		CanvasUIMethodCall_1_t997E0BDAED4D18797819B03656FEE2BD27FF80C6* L_22 = L_21;
		FacebookDelegate_1_t196A2AB9CCB2BC5DCA5BC05F82516E4C3FF9DD4B* L_23 = ___10_callback;
		NullCheck(L_22);
		MethodCall_1_set_Callback_m2FE69B0E3B56B162D1F585BF08320F54F3EB73CB_inline(L_22, L_23, MethodCall_1_set_Callback_m2FE69B0E3B56B162D1F585BF08320F54F3EB73CB_RuntimeMethod_var);
		MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* L_24 = V_0;
		NullCheck(L_22);
		VirtualActionInvoker1< MethodArguments_tE5E9B6635A357E451FF2324D5BE816910F0074BD* >::Invoke(4 /* System.Void Facebook.Unity.MethodCall`1<Facebook.Unity.IPayResult>::Call(Facebook.Unity.MethodArguments) */, L_22, L_24);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook::<OnLoginComplete>b__37_0(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebook_U3COnLoginCompleteU3Eb__37_0_mECC453F16A4ABDD1C5301986A3265C98AA4F3945 (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_formattedResponse, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_0 = ___0_formattedResponse;
		LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1* L_1 = (LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1*)il2cpp_codegen_object_new(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		LoginResult__ctor_m872B7ED7617120053FBB543555ECE68D51E04448(L_1, L_0, NULL);
		VirtualActionInvoker1< LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1* >::Invoke(49 /* System.Void Facebook.Unity.FacebookBase::OnAuthResponse(Facebook.Unity.LoginResult) */, __this, L_1);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void Facebook.Unity.Canvas.CanvasFacebook/<>c::.cctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__cctor_m301ECE813EC06AA199DBD1D27A011D77D58D57AD (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8* L_0 = (U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8*)il2cpp_codegen_object_new(U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		U3CU3Ec__ctor_m9E708C2F94EE7BA080505D395F1D5B63F00D64FA(L_0, NULL);
		((U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var))->___U3CU3E9_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8_il2cpp_TypeInfo_var))->___U3CU3E9_0), (void*)L_0);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook/<>c::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__ctor_m9E708C2F94EE7BA080505D395F1D5B63F00D64FA (U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook/<>c::<OnFacebookAuthResponseChange>b__40_0(Facebook.Unity.ResultContainer)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec_U3COnFacebookAuthResponseChangeU3Eb__40_0_m6BDA2E988B1B4F39B882D8A24DE94F228F1B481F (U3CU3Ec_t2C9BA0FCFE0DFEA4516C51A9AB8736E2377BC5B8* __this, ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* ___0_formattedResponse, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_0 = ___0_formattedResponse;
		LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1* L_1 = (LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1*)il2cpp_codegen_object_new(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		LoginResult__ctor_m872B7ED7617120053FBB543555ECE68D51E04448(L_1, L_0, NULL);
		NullCheck(L_1);
		AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346* L_2;
		L_2 = LoginResult_get_AccessToken_m57A8C7CD5C7AE61792EEBBC7486FBD18C03EB7FE_inline(L_1, NULL);
		AccessToken_set_CurrentAccessToken_m7C9C5DF205C036C288F21657E7891FF461BE9EB4_inline(L_2, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void Facebook.Unity.Canvas.CanvasFacebook/<>c__DisplayClass47_0::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__DisplayClass47_0__ctor_m8D4983F5B36F90967DDFDD0051DBDD7BE6E39DE7 (U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebook/<>c__DisplayClass47_0::<FormatAuthResponse>b__0(Facebook.Unity.IGraphResult)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__DisplayClass47_0_U3CFormatAuthResponseU3Eb__0_m601241C800917FCB891C39BCCEA9988624FDE94A (U3CU3Ec__DisplayClass47_0_tC325FD2BA234BC428DF99019A297F7E70389DF4A* __this, RuntimeObject* ___0_r, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FacebookLogger_tAF941780684648D82AFA6B5D95965FAAFBBDB457_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICollection_1_t5C03FBFD5ECBDE4EAB8C4ED582DDFCF702EB5DC7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IDisposable_t030E0496B4E0E4E4F086825007979AF51F7248C5_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IEnumerable_1_tF95C9E01A913DD50575531C8305932628663D9E9_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IEnumerator_1_t43D2E4BA9246755F293DFA74F001FB1A70A648FD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IEnumerator_t7B609C2FFA6EB5167D9C62A0C32A21DE2F666DAA_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IResult_tDB97C9EEF7759E32ACC78D2DD9B7B0D4E42B67B6_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&List_1__ctor_mCA8DD57EAC70C2B5923DBB9D5A77CEAC22E7068E_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Utilities_TryGetValue_TisIDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_mF4F1612A6CC71E09B0A940E7B840C94637D8BB97_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Utilities_TryGetValue_TisIList_1_t6EE90D273EFCF5E7E4C37FAB712E70BB6F1B4BFF_mC232E1497C9BEC609103DB1B2BE3EDF69B141C16_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Utilities_TryGetValue_TisString_t_m2341652CD88C1BF40208CFE6D13B0EF59437D8CC_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral0B09BDDCF49998D084C7F5812B74CC75E585E07E);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral126A04A5A066C00B77AB4F3483884A7606F6BC5D);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral30A7BF1FEE5DF8662ED83E80AA4537E34CC0361B);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral3F3FFF195FC51E628335B6613953826BB06DAEB4);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral46C0DBF8E329C836732C7C6705567D71C6792B50);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4BB4572C6AC73940D49948BC96E824C0CFA913C8);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE79F08E3990626B5F3144E992D4D2F7D14584EC0);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE8DE737330234B3EAC92FAE2AAB6B7DB5326CB91);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralFD4706B02823C71252FBF63A74CF03433A8DADF0);
		s_Il2CppMethodInitialized = true;
	}
	RuntimeObject* V_0 = NULL;
	RuntimeObject* V_1 = NULL;
	RuntimeObject* V_2 = NULL;
	RuntimeObject* V_3 = NULL;
	RuntimeObject* V_4 = NULL;
	String_t* V_5 = NULL;
	String_t* V_6 = NULL;
	{
		RuntimeObject* L_0 = ___0_r;
		NullCheck(L_0);
		RuntimeObject* L_1;
		L_1 = InterfaceFuncInvoker0< RuntimeObject* >::Invoke(1 /* System.Collections.Generic.IDictionary`2<System.String,System.Object> Facebook.Unity.IResult::get_ResultDictionary() */, IResult_tDB97C9EEF7759E32ACC78D2DD9B7B0D4E42B67B6_il2cpp_TypeInfo_var, L_0);
		if (!L_1)
		{
			goto IL_00ea;
		}
	}
	{
		RuntimeObject* L_2 = ___0_r;
		NullCheck(L_2);
		RuntimeObject* L_3;
		L_3 = InterfaceFuncInvoker0< RuntimeObject* >::Invoke(1 /* System.Collections.Generic.IDictionary`2<System.String,System.Object> Facebook.Unity.IResult::get_ResultDictionary() */, IResult_tDB97C9EEF7759E32ACC78D2DD9B7B0D4E42B67B6_il2cpp_TypeInfo_var, L_2);
		bool L_4;
		L_4 = Utilities_TryGetValue_TisIDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_mF4F1612A6CC71E09B0A940E7B840C94637D8BB97(L_3, _stringLiteralE79F08E3990626B5F3144E992D4D2F7D14584EC0, (&V_0), Utilities_TryGetValue_TisIDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_mF4F1612A6CC71E09B0A940E7B840C94637D8BB97_RuntimeMethod_var);
		if (!L_4)
		{
			goto IL_00ea;
		}
	}
	{
		List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* L_5 = (List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD*)il2cpp_codegen_object_new(List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD_il2cpp_TypeInfo_var);
		NullCheck(L_5);
		List_1__ctor_mCA8DD57EAC70C2B5923DBB9D5A77CEAC22E7068E(L_5, List_1__ctor_mCA8DD57EAC70C2B5923DBB9D5A77CEAC22E7068E_RuntimeMethod_var);
		V_1 = L_5;
		RuntimeObject* L_6 = V_0;
		bool L_7;
		L_7 = Utilities_TryGetValue_TisIList_1_t6EE90D273EFCF5E7E4C37FAB712E70BB6F1B4BFF_mC232E1497C9BEC609103DB1B2BE3EDF69B141C16(L_6, _stringLiteralA44A39671D4B7FA8FBE50D795EAB52248D5C5469, (&V_2), Utilities_TryGetValue_TisIList_1_t6EE90D273EFCF5E7E4C37FAB712E70BB6F1B4BFF_mC232E1497C9BEC609103DB1B2BE3EDF69B141C16_RuntimeMethod_var);
		if (!L_7)
		{
			goto IL_00c3;
		}
	}
	{
		RuntimeObject* L_8 = V_2;
		NullCheck(L_8);
		RuntimeObject* L_9;
		L_9 = InterfaceFuncInvoker0< RuntimeObject* >::Invoke(0 /* System.Collections.Generic.IEnumerator`1<T> System.Collections.Generic.IEnumerable`1<System.Object>::GetEnumerator() */, IEnumerable_1_tF95C9E01A913DD50575531C8305932628663D9E9_il2cpp_TypeInfo_var, L_8);
		V_3 = L_9;
	}
	{
		auto __finallyBlock = il2cpp::utils::Finally([&]
		{

FINALLY_00b9:
			{// begin finally (depth: 1)
				{
					RuntimeObject* L_10 = V_3;
					if (!L_10)
					{
						goto IL_00c2;
					}
				}
				{
					RuntimeObject* L_11 = V_3;
					NullCheck(L_11);
					InterfaceActionInvoker0::Invoke(0 /* System.Void System.IDisposable::Dispose() */, IDisposable_t030E0496B4E0E4E4F086825007979AF51F7248C5_il2cpp_TypeInfo_var, L_11);
				}

IL_00c2:
				{
					return;
				}
			}// end finally (depth: 1)
		});
		try
		{// begin try (depth: 1)
			{
				goto IL_00af_1;
			}

IL_0043_1:
			{
				RuntimeObject* L_12 = V_3;
				NullCheck(L_12);
				RuntimeObject* L_13;
				L_13 = InterfaceFuncInvoker0< RuntimeObject* >::Invoke(0 /* T System.Collections.Generic.IEnumerator`1<System.Object>::get_Current() */, IEnumerator_1_t43D2E4BA9246755F293DFA74F001FB1A70A648FD_il2cpp_TypeInfo_var, L_12);
				V_4 = ((RuntimeObject*)IsInst((RuntimeObject*)L_13, IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var));
				RuntimeObject* L_14 = V_4;
				if (!L_14)
				{
					goto IL_00a5_1;
				}
			}
			{
				RuntimeObject* L_15 = V_4;
				bool L_16;
				L_16 = Utilities_TryGetValue_TisString_t_m2341652CD88C1BF40208CFE6D13B0EF59437D8CC(L_15, _stringLiteralFD4706B02823C71252FBF63A74CF03433A8DADF0, (&V_5), Utilities_TryGetValue_TisString_t_m2341652CD88C1BF40208CFE6D13B0EF59437D8CC_RuntimeMethod_var);
				if (!L_16)
				{
					goto IL_0099_1;
				}
			}
			{
				String_t* L_17 = V_5;
				NullCheck(L_17);
				bool L_18;
				L_18 = String_Equals_m7BDFC0B951005B9DC2BAED464AFE68FF7E9ACE5A(L_17, _stringLiteral30A7BF1FEE5DF8662ED83E80AA4537E34CC0361B, 3, NULL);
				if (!L_18)
				{
					goto IL_0099_1;
				}
			}
			{
				RuntimeObject* L_19 = V_4;
				bool L_20;
				L_20 = Utilities_TryGetValue_TisString_t_m2341652CD88C1BF40208CFE6D13B0EF59437D8CC(L_19, _stringLiteral126A04A5A066C00B77AB4F3483884A7606F6BC5D, (&V_6), Utilities_TryGetValue_TisString_t_m2341652CD88C1BF40208CFE6D13B0EF59437D8CC_RuntimeMethod_var);
				if (!L_20)
				{
					goto IL_008d_1;
				}
			}
			{
				RuntimeObject* L_21 = V_1;
				String_t* L_22 = V_6;
				NullCheck(L_21);
				InterfaceActionInvoker1< String_t* >::Invoke(2 /* System.Void System.Collections.Generic.ICollection`1<System.String>::Add(T) */, ICollection_1_t5C03FBFD5ECBDE4EAB8C4ED582DDFCF702EB5DC7_il2cpp_TypeInfo_var, L_21, L_22);
				goto IL_00af_1;
			}

IL_008d_1:
			{
				il2cpp_codegen_runtime_class_init_inline(FacebookLogger_tAF941780684648D82AFA6B5D95965FAAFBBDB457_il2cpp_TypeInfo_var);
				FacebookLogger_Warn_mDAD4A3F15FE52EF33DDC7A7B5639E04F057509D9(_stringLiteral3F3FFF195FC51E628335B6613953826BB06DAEB4, NULL);
				goto IL_00af_1;
			}

IL_0099_1:
			{
				il2cpp_codegen_runtime_class_init_inline(FacebookLogger_tAF941780684648D82AFA6B5D95965FAAFBBDB457_il2cpp_TypeInfo_var);
				FacebookLogger_Warn_mDAD4A3F15FE52EF33DDC7A7B5639E04F057509D9(_stringLiteral46C0DBF8E329C836732C7C6705567D71C6792B50, NULL);
				goto IL_00af_1;
			}

IL_00a5_1:
			{
				il2cpp_codegen_runtime_class_init_inline(FacebookLogger_tAF941780684648D82AFA6B5D95965FAAFBBDB457_il2cpp_TypeInfo_var);
				FacebookLogger_Warn_mDAD4A3F15FE52EF33DDC7A7B5639E04F057509D9(_stringLiteral4BB4572C6AC73940D49948BC96E824C0CFA913C8, NULL);
			}

IL_00af_1:
			{
				RuntimeObject* L_23 = V_3;
				NullCheck(L_23);
				bool L_24;
				L_24 = InterfaceFuncInvoker0< bool >::Invoke(0 /* System.Boolean System.Collections.IEnumerator::MoveNext() */, IEnumerator_t7B609C2FFA6EB5167D9C62A0C32A21DE2F666DAA_il2cpp_TypeInfo_var, L_23);
				if (L_24)
				{
					goto IL_0043_1;
				}
			}
			{
				goto IL_00cd;
			}
		}// end try (depth: 1)
		catch(Il2CppExceptionWrapper& e)
		{
			__finallyBlock.StoreException(e.ex);
		}
	}

IL_00c3:
	{
		il2cpp_codegen_runtime_class_init_inline(FacebookLogger_tAF941780684648D82AFA6B5D95965FAAFBBDB457_il2cpp_TypeInfo_var);
		FacebookLogger_Warn_mDAD4A3F15FE52EF33DDC7A7B5639E04F057509D9(_stringLiteral0B09BDDCF49998D084C7F5812B74CC75E585E07E, NULL);
	}

IL_00cd:
	{
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_25 = __this->___result_0;
		NullCheck(L_25);
		RuntimeObject* L_26;
		L_26 = ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline(L_25, NULL);
		il2cpp_codegen_runtime_class_init_inline(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var);
		String_t* L_27 = ((LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_StaticFields*)il2cpp_codegen_static_fields_for(LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1_il2cpp_TypeInfo_var))->___PermissionsKey_9;
		RuntimeObject* L_28 = V_1;
		String_t* L_29;
		L_29 = Utilities_ToCommaSeparateList_mCB4781E5AB2247A25AC99812FC901A171EDD747B(L_28, NULL);
		NullCheck(L_26);
		InterfaceActionInvoker2< String_t*, RuntimeObject* >::Invoke(1 /* System.Void System.Collections.Generic.IDictionary`2<System.String,System.Object>::set_Item(TKey,TValue) */, IDictionary_2_t79D4ADB15B238AC117DF72982FEA3C42EF5AFA19_il2cpp_TypeInfo_var, L_26, L_27, L_29);
		goto IL_00f4;
	}

IL_00ea:
	{
		il2cpp_codegen_runtime_class_init_inline(FacebookLogger_tAF941780684648D82AFA6B5D95965FAAFBBDB457_il2cpp_TypeInfo_var);
		FacebookLogger_Warn_mDAD4A3F15FE52EF33DDC7A7B5639E04F057509D9(_stringLiteralE8DE737330234B3EAC92FAE2AAB6B7DB5326CB91, NULL);
	}

IL_00f4:
	{
		Callback_1_tFC22DABD78A3E51786E5CDB3C3A5B523E804BB92* L_30 = __this->___callback_1;
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_31 = __this->___result_0;
		NullCheck(L_30);
		Callback_1_Invoke_mFD8528526031568E661F81643EF709689D3884B0_inline(L_30, L_31, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Facebook.Unity.Canvas.ICanvasFacebookImplementation Facebook.Unity.Canvas.CanvasFacebookGameObject::get_CanvasFacebookImpl()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* CanvasFacebookGameObject_get_CanvasFacebookImpl_m820D790D560FE4C671A11206517112613C721FA0 (CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasFacebookImplementation_t26FCCCC1DEF6E943A189D145F7FB7E4E5B90A38C_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0;
		L_0 = FacebookGameObject_get_Facebook_m8F6DC9F80E732D237D7F858FB1FC5D448071D137_inline(__this, NULL);
		return ((RuntimeObject*)Castclass((RuntimeObject*)L_0, ICanvasFacebookImplementation_t26FCCCC1DEF6E943A189D145F7FB7E4E5B90A38C_il2cpp_TypeInfo_var));
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebookGameObject::OnPayComplete(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebookGameObject_OnPayComplete_mAD590A57E2A03A31255C76A651024B1DFE94ADB9 (CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* __this, String_t* ___0_result, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasFacebookResultHandler_t0A9297E8C6A10D56FF83AAF579422BC153AAE54E_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0;
		L_0 = CanvasFacebookGameObject_get_CanvasFacebookImpl_m820D790D560FE4C671A11206517112613C721FA0(__this, NULL);
		String_t* L_1 = ___0_result;
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_2 = (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B*)il2cpp_codegen_object_new(ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B_il2cpp_TypeInfo_var);
		NullCheck(L_2);
		ResultContainer__ctor_mAD6B7ADD48D8244E67DEC442276642A1DB0EB74C(L_2, L_1, NULL);
		NullCheck(L_0);
		InterfaceActionInvoker1< ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* >::Invoke(0 /* System.Void Facebook.Unity.Canvas.ICanvasFacebookResultHandler::OnPayComplete(Facebook.Unity.ResultContainer) */, ICanvasFacebookResultHandler_t0A9297E8C6A10D56FF83AAF579422BC153AAE54E_il2cpp_TypeInfo_var, L_0, L_2);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebookGameObject::OnFacebookAuthResponseChange(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebookGameObject_OnFacebookAuthResponseChange_m7A8C55480D8BBE68D39F28604FBEEFE119AAC8FE (CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* __this, String_t* ___0_message, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasFacebookResultHandler_t0A9297E8C6A10D56FF83AAF579422BC153AAE54E_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0;
		L_0 = CanvasFacebookGameObject_get_CanvasFacebookImpl_m820D790D560FE4C671A11206517112613C721FA0(__this, NULL);
		String_t* L_1 = ___0_message;
		ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* L_2 = (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B*)il2cpp_codegen_object_new(ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B_il2cpp_TypeInfo_var);
		NullCheck(L_2);
		ResultContainer__ctor_mAD6B7ADD48D8244E67DEC442276642A1DB0EB74C(L_2, L_1, NULL);
		NullCheck(L_0);
		InterfaceActionInvoker1< ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* >::Invoke(1 /* System.Void Facebook.Unity.Canvas.ICanvasFacebookResultHandler::OnFacebookAuthResponseChange(Facebook.Unity.ResultContainer) */, ICanvasFacebookResultHandler_t0A9297E8C6A10D56FF83AAF579422BC153AAE54E_il2cpp_TypeInfo_var, L_0, L_2);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebookGameObject::OnUrlResponse(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebookGameObject_OnUrlResponse_m2B810CEBB06EF9BC30C520EF73AE2C9E6A4893E0 (CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* __this, String_t* ___0_message, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasFacebookResultHandler_t0A9297E8C6A10D56FF83AAF579422BC153AAE54E_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0;
		L_0 = CanvasFacebookGameObject_get_CanvasFacebookImpl_m820D790D560FE4C671A11206517112613C721FA0(__this, NULL);
		String_t* L_1 = ___0_message;
		NullCheck(L_0);
		InterfaceActionInvoker1< String_t* >::Invoke(2 /* System.Void Facebook.Unity.Canvas.ICanvasFacebookResultHandler::OnUrlResponse(System.String) */, ICanvasFacebookResultHandler_t0A9297E8C6A10D56FF83AAF579422BC153AAE54E_il2cpp_TypeInfo_var, L_0, L_1);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebookGameObject::OnHideUnity(System.Boolean)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebookGameObject_OnHideUnity_m559EF49590A408C9E4FF3205D41EAD9187DBDBBB (CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* __this, bool ___0_hide, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasFacebookResultHandler_t0A9297E8C6A10D56FF83AAF579422BC153AAE54E_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0;
		L_0 = CanvasFacebookGameObject_get_CanvasFacebookImpl_m820D790D560FE4C671A11206517112613C721FA0(__this, NULL);
		bool L_1 = ___0_hide;
		NullCheck(L_0);
		InterfaceActionInvoker1< bool >::Invoke(3 /* System.Void Facebook.Unity.Canvas.ICanvasFacebookResultHandler::OnHideUnity(System.Boolean) */, ICanvasFacebookResultHandler_t0A9297E8C6A10D56FF83AAF579422BC153AAE54E_il2cpp_TypeInfo_var, L_0, L_1);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebookGameObject::OnAwake()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebookGameObject_OnAwake_mC286FDC276190169D2D006C8AB21A807FFDB12C0 (CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&GameObject_AddComponent_TisJsBridge_t338341DF8272C935803F827012749337260971DA_mE386CD7945F07943362FF25193BDB880EAFE1AE3_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&GameObject_t76FEDD663AB33C991A9C9A23129337651094216F_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral9099FF4365E05FD8FF479542BF93799DD19F2D40);
		s_Il2CppMethodInitialized = true;
	}
	{
		GameObject_t76FEDD663AB33C991A9C9A23129337651094216F* L_0 = (GameObject_t76FEDD663AB33C991A9C9A23129337651094216F*)il2cpp_codegen_object_new(GameObject_t76FEDD663AB33C991A9C9A23129337651094216F_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		GameObject__ctor_m37D512B05D292F954792225E6C6EEE95293A9B88(L_0, _stringLiteral9099FF4365E05FD8FF479542BF93799DD19F2D40, NULL);
		GameObject_t76FEDD663AB33C991A9C9A23129337651094216F* L_1 = L_0;
		NullCheck(L_1);
		JsBridge_t338341DF8272C935803F827012749337260971DA* L_2;
		L_2 = GameObject_AddComponent_TisJsBridge_t338341DF8272C935803F827012749337260971DA_mE386CD7945F07943362FF25193BDB880EAFE1AE3(L_1, GameObject_AddComponent_TisJsBridge_t338341DF8272C935803F827012749337260971DA_mE386CD7945F07943362FF25193BDB880EAFE1AE3_RuntimeMethod_var);
		NullCheck(L_1);
		Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* L_3;
		L_3 = GameObject_get_transform_m0BC10ADFA1632166AE5544BDF9038A2650C2AE56(L_1, NULL);
		GameObject_t76FEDD663AB33C991A9C9A23129337651094216F* L_4;
		L_4 = Component_get_gameObject_m57AEFBB14DB39EC476F740BA000E170355DE691B(__this, NULL);
		NullCheck(L_4);
		Transform_tB27202C6F4E36D225EE28A13E4D662BF99785DB1* L_5;
		L_5 = GameObject_get_transform_m0BC10ADFA1632166AE5544BDF9038A2650C2AE56(L_4, NULL);
		NullCheck(L_3);
		Transform_set_parent_m9BD5E563B539DD5BEC342736B03F97B38A243234(L_3, L_5, NULL);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebookGameObject::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebookGameObject__ctor_m6BD8741949A8FB687ECBD64C50A36862A6B3BCC1 (CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* __this, const RuntimeMethod* method) 
{
	{
		FacebookGameObject__ctor_mA095CF29FC6CD7D4C5712F356B75C0F845C954AF(__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// Facebook.Unity.FacebookGameObject Facebook.Unity.Canvas.CanvasFacebookLoader::get_FBGameObject()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B* CanvasFacebookLoader_get_FBGameObject_m0A400611CEF5A3F63113A889C140F7C312BED86B (CanvasFacebookLoader_t50D8266C527416BEBCBA1355E4513AD3DEBC71EA* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ComponentFactory_GetComponent_TisCanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46_mE51621647A97AC742D31A41B486013594290ED7B_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* V_0 = NULL;
	{
		CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* L_0;
		L_0 = ComponentFactory_GetComponent_TisCanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46_mE51621647A97AC742D31A41B486013594290ED7B(0, ComponentFactory_GetComponent_TisCanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46_mE51621647A97AC742D31A41B486013594290ED7B_RuntimeMethod_var);
		V_0 = L_0;
		CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* L_1 = V_0;
		NullCheck(L_1);
		RuntimeObject* L_2;
		L_2 = FacebookGameObject_get_Facebook_m8F6DC9F80E732D237D7F858FB1FC5D448071D137_inline(L_1, NULL);
		if (L_2)
		{
			goto IL_001a;
		}
	}
	{
		CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* L_3 = V_0;
		CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1* L_4 = (CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1*)il2cpp_codegen_object_new(CanvasFacebook_t456AE335836BFFB54B0D77A3B4CF04D7D830A6E1_il2cpp_TypeInfo_var);
		NullCheck(L_4);
		CanvasFacebook__ctor_m84A06718D125F7DEC19BD186F5D318F495248EFE(L_4, NULL);
		NullCheck(L_3);
		FacebookGameObject_set_Facebook_mEDCEAC5301992E16A962C3993F25C151F6251F1C_inline(L_3, L_4, NULL);
	}

IL_001a:
	{
		CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* L_5 = V_0;
		return L_5;
	}
}
// System.Void Facebook.Unity.Canvas.CanvasFacebookLoader::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void CanvasFacebookLoader__ctor_mB07FF0671755BEAE1509D5E75A64137A31A8FD87 (CanvasFacebookLoader_t50D8266C527416BEBCBA1355E4513AD3DEBC71EA* __this, const RuntimeMethod* method) 
{
	{
		CompiledFacebookLoader__ctor_m0CEF02AE4B19FCB945491FD9DE8B4CAB0CB4F227(__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// System.Void Facebook.Unity.Canvas.JsBridge::Start()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge_Start_mAAF7181E2A872FC8C742EC6D7C796347F4DEBB1E (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ComponentFactory_GetComponent_TisCanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46_mE51621647A97AC742D31A41B486013594290ED7B_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		CanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46* L_0;
		L_0 = ComponentFactory_GetComponent_TisCanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46_mE51621647A97AC742D31A41B486013594290ED7B(1, ComponentFactory_GetComponent_TisCanvasFacebookGameObject_tCEF960711D27D55DC0CD48CCB5793B6E96557D46_mE51621647A97AC742D31A41B486013594290ED7B_RuntimeMethod_var);
		__this->___facebook_4 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___facebook_4), (void*)L_0);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.JsBridge::OnLoginComplete(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge_OnLoginComplete_m478A8D70CF7AF78EEE38D27604836B6AB7A2267C (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, String_t* ___0_responseJsonData, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IFacebookCallbackHandler_t3D24BE45B97C4B3E6BD94E249E3E9D0C27CC914D_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___facebook_4;
		String_t* L_1 = ___0_responseJsonData;
		NullCheck(L_0);
		InterfaceActionInvoker1< String_t* >::Invoke(1 /* System.Void Facebook.Unity.IFacebookCallbackHandler::OnLoginComplete(System.String) */, IFacebookCallbackHandler_t3D24BE45B97C4B3E6BD94E249E3E9D0C27CC914D_il2cpp_TypeInfo_var, L_0, L_1);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.JsBridge::OnFacebookAuthResponseChange(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge_OnFacebookAuthResponseChange_m2D1A480A6CD1F2890EB04B4A5557E8EDC6AA9898 (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, String_t* ___0_responseJsonData, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___facebook_4;
		String_t* L_1 = ___0_responseJsonData;
		NullCheck(L_0);
		InterfaceActionInvoker1< String_t* >::Invoke(1 /* System.Void Facebook.Unity.Canvas.ICanvasFacebookCallbackHandler::OnFacebookAuthResponseChange(System.String) */, ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271_il2cpp_TypeInfo_var, L_0, L_1);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.JsBridge::OnPayComplete(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge_OnPayComplete_m9255C062C0720D77399170CF8C8E247DCD863952 (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, String_t* ___0_responseJsonData, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___facebook_4;
		String_t* L_1 = ___0_responseJsonData;
		NullCheck(L_0);
		InterfaceActionInvoker1< String_t* >::Invoke(0 /* System.Void Facebook.Unity.Canvas.ICanvasFacebookCallbackHandler::OnPayComplete(System.String) */, ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271_il2cpp_TypeInfo_var, L_0, L_1);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.JsBridge::OnAppRequestsComplete(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge_OnAppRequestsComplete_m1C77062A6F2F3129AE85B6E550881E708925F198 (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, String_t* ___0_responseJsonData, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IFacebookCallbackHandler_t3D24BE45B97C4B3E6BD94E249E3E9D0C27CC914D_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___facebook_4;
		String_t* L_1 = ___0_responseJsonData;
		NullCheck(L_0);
		InterfaceActionInvoker1< String_t* >::Invoke(2 /* System.Void Facebook.Unity.IFacebookCallbackHandler::OnAppRequestsComplete(System.String) */, IFacebookCallbackHandler_t3D24BE45B97C4B3E6BD94E249E3E9D0C27CC914D_il2cpp_TypeInfo_var, L_0, L_1);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.JsBridge::OnShareLinkComplete(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge_OnShareLinkComplete_mFB16F927CEA780C609E119455825568057F0A590 (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, String_t* ___0_responseJsonData, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IFacebookCallbackHandler_t3D24BE45B97C4B3E6BD94E249E3E9D0C27CC914D_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___facebook_4;
		String_t* L_1 = ___0_responseJsonData;
		NullCheck(L_0);
		InterfaceActionInvoker1< String_t* >::Invoke(3 /* System.Void Facebook.Unity.IFacebookCallbackHandler::OnShareLinkComplete(System.String) */, IFacebookCallbackHandler_t3D24BE45B97C4B3E6BD94E249E3E9D0C27CC914D_il2cpp_TypeInfo_var, L_0, L_1);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.JsBridge::OnFacebookFocus(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge_OnFacebookFocus_m0C7639DE1243602FD595AF54E2DEB9F5723E7C54 (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, String_t* ___0_state, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral31D307275CC464AFDCC4A193A3D0DADE7D308F81);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___facebook_4;
		String_t* L_1 = ___0_state;
		bool L_2;
		L_2 = String_op_Inequality_m8C940F3CFC42866709D7CA931B3D77B4BE94BCB6(L_1, _stringLiteral31D307275CC464AFDCC4A193A3D0DADE7D308F81, NULL);
		NullCheck(L_0);
		InterfaceActionInvoker1< bool >::Invoke(3 /* System.Void Facebook.Unity.Canvas.ICanvasFacebookCallbackHandler::OnHideUnity(System.Boolean) */, ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271_il2cpp_TypeInfo_var, L_0, L_2);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.JsBridge::OnInitComplete(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge_OnInitComplete_m5B8598453F58C4E156123BCE47D9C1C54DBCB4B8 (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, String_t* ___0_responseJsonData, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IFacebookCallbackHandler_t3D24BE45B97C4B3E6BD94E249E3E9D0C27CC914D_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___facebook_4;
		String_t* L_1 = ___0_responseJsonData;
		NullCheck(L_0);
		InterfaceActionInvoker1< String_t* >::Invoke(0 /* System.Void Facebook.Unity.IFacebookCallbackHandler::OnInitComplete(System.String) */, IFacebookCallbackHandler_t3D24BE45B97C4B3E6BD94E249E3E9D0C27CC914D_il2cpp_TypeInfo_var, L_0, L_1);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.JsBridge::OnUrlResponse(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge_OnUrlResponse_m701C53BF71C5BA4A050C985C90EDBF0E21027BA3 (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, String_t* ___0_url, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		RuntimeObject* L_0 = __this->___facebook_4;
		String_t* L_1 = ___0_url;
		NullCheck(L_0);
		InterfaceActionInvoker1< String_t* >::Invoke(2 /* System.Void Facebook.Unity.Canvas.ICanvasFacebookCallbackHandler::OnUrlResponse(System.String) */, ICanvasFacebookCallbackHandler_t0ABD5CC9558BBEF6D1E0448BB5A1F7A88214E271_il2cpp_TypeInfo_var, L_0, L_1);
		return;
	}
}
// System.Void Facebook.Unity.Canvas.JsBridge::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void JsBridge__ctor_m8104A953BEA1CC11145810A1F2EBC91574A1D29B (JsBridge_t338341DF8272C935803F827012749337260971DA* __this, const RuntimeMethod* method) 
{
	{
		MonoBehaviour__ctor_m592DB0105CA0BC97AA1C5F4AD27B12D68A3B7C1E(__this, NULL);
		return;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void NativeDict_set_NumEntries_m57353835AD86ED502693E4BE8DCE1E3265BD6B52_inline (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, int32_t ___0_value, const RuntimeMethod* method) 
{
	{
		int32_t L_0 = ___0_value;
		__this->___U3CNumEntriesU3Ek__BackingField_0 = L_0;
		return;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void NativeDict_set_Keys_m30955DED0955CCE506D1FC315F2F534BF1450D7D_inline (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___0_value, const RuntimeMethod* method) 
{
	{
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_0 = ___0_value;
		__this->___U3CKeysU3Ek__BackingField_1 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CKeysU3Ek__BackingField_1), (void*)L_0);
		return;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void NativeDict_set_Values_m2D082203061E2C96CD3F728152BB43361E3CD860_inline (NativeDict_t4A202CF8A258D4786D791217AEBBC54F2B88EFA9* __this, StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___0_value, const RuntimeMethod* method) 
{
	{
		StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* L_0 = ___0_value;
		__this->___U3CValuesU3Ek__BackingField_2 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CValuesU3Ek__BackingField_2), (void*)L_0);
		return;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* FacebookGameObject_get_Facebook_m8F6DC9F80E732D237D7F858FB1FC5D448071D137_inline (FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B* __this, const RuntimeMethod* method) 
{
	{
		RuntimeObject* L_0 = __this->___U3CFacebookU3Ek__BackingField_4;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void FacebookGameObject_set_Facebook_mEDCEAC5301992E16A962C3993F25C151F6251F1C_inline (FacebookGameObject_t2A844C47156B1DC094D9D008FFFC2F730F8E0D0B* __this, RuntimeObject* ___0_value, const RuntimeMethod* method) 
{
	{
		RuntimeObject* L_0 = ___0_value;
		__this->___U3CFacebookU3Ek__BackingField_4 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CFacebookU3Ek__BackingField_4), (void*)L_0);
		return;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void AndroidFacebook_set_KeyHash_m955883E7FF2093A8219358F222D292B3E556FED1_inline (AndroidFacebook_tF88E54F7B5DA2E5974F0A7291E52F91F42029ADD* __this, String_t* ___0_value, const RuntimeMethod* method) 
{
	{
		String_t* L_0 = ___0_value;
		__this->___U3CKeyHashU3Ek__BackingField_7 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CKeyHashU3Ek__BackingField_7), (void*)L_0);
		return;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void FacebookBase_Init_m20FB2F18FDA2C3A4FA3EAD6C734EEFEAA5CF5F6A_inline (FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* __this, InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F* ___0_onInitComplete, const RuntimeMethod* method) 
{
	{
		InitDelegate_t880BF96D9E733404D1E36BF894DDA83C1B9A1A9F* L_0 = ___0_onInitComplete;
		__this->___onInitCompleteDelegate_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___onInitCompleteDelegate_0), (void*)L_0);
		return;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* FB_get_GraphApiVersion_m234E4C9258DE0F012E1A78D5A4DED1DE8D6D5198_inline (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&FB_tD6AF917A642BEC6920761C8E4AD4013414829013_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		il2cpp_codegen_runtime_class_init_inline(FB_tD6AF917A642BEC6920761C8E4AD4013414829013_il2cpp_TypeInfo_var);
		String_t* L_0 = ((FB_tD6AF917A642BEC6920761C8E4AD4013414829013_StaticFields*)il2cpp_codegen_static_fields_for(FB_tD6AF917A642BEC6920761C8E4AD4013414829013_il2cpp_TypeInfo_var))->___graphApiVersion_9;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* FacebookBase_get_CallbackManager_m6C4BEBEF920CD139CF777D45E0E924829E4CF57F_inline (FacebookBase_tF5635ED76AFFFA9234A516FCD6AAF6C6B55B5865* __this, const RuntimeMethod* method) 
{
	{
		CallbackManager_t3B9141B4E44116445C556786C02DEDA7CE612749* L_0 = __this->___U3CCallbackManagerU3Ek__BackingField_2;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void HideUnityDelegate_Invoke_m84D79F9CD775257293DF8E35297F43835E18CD71_inline (HideUnityDelegate_t73424171C1A0762619208C0090DD84BA51FF9BCE* __this, bool ___0_isUnityShown, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, bool, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl_1)((Il2CppObject*)__this->___method_code_6, ___0_isUnityShown, reinterpret_cast<RuntimeMethod*>(__this->___method_3));
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* ResultContainer_get_ResultDictionary_m76808545CDB3106D7129B3E90F9F9C027DFE16BD_inline (ResultContainer_tCEBF6F181CFDC82B929D1BA360D7C6EEE46D321B* __this, const RuntimeMethod* method) 
{
	{
		RuntimeObject* L_0 = __this->___U3CResultDictionaryU3Ek__BackingField_2;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346* LoginResult_get_AccessToken_m57A8C7CD5C7AE61792EEBBC7486FBD18C03EB7FE_inline (LoginResult_t14BC55B035322862D91FCB9D24D071953DAC09C1* __this, const RuntimeMethod* method) 
{
	{
		AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346* L_0 = __this->___U3CAccessTokenU3Ek__BackingField_14;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void AccessToken_set_CurrentAccessToken_m7C9C5DF205C036C288F21657E7891FF461BE9EB4_inline (AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346* ___0_value, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346* L_0 = ___0_value;
		((AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346_StaticFields*)il2cpp_codegen_static_fields_for(AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346_il2cpp_TypeInfo_var))->___U3CCurrentAccessTokenU3Ek__BackingField_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346_StaticFields*)il2cpp_codegen_static_fields_for(AccessToken_t73BAE9F45493316E7FF33A14EECB3D77E8C0D346_il2cpp_TypeInfo_var))->___U3CCurrentAccessTokenU3Ek__BackingField_0), (void*)L_0);
		return;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void MethodCall_1_set_Callback_mA74BB403FFEBB049BA981D103C39772844B08762_gshared_inline (MethodCall_1_tBE309A8BE882A6A1F80E42A8EE1493B9CB4FBBC8* __this, FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9* ___0_value, const RuntimeMethod* method) 
{
	{
		FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9* L_0 = ___0_value;
		__this->___U3CCallbackU3Ek__BackingField_1 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CCallbackU3Ek__BackingField_1), (void*)L_0);
		return;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Nullable_1_get_HasValue_mC149B1C717AF506BBE8932F2C1DC86C378D17EA8_gshared_inline (Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75* __this, const RuntimeMethod* method) 
{
	{
		bool L_0 = (bool)__this->___hasValue_0;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR float Nullable_1_GetValueOrDefault_m068A148705ED1E215A5E85D18BA6852B192DA419_gshared_inline (Nullable_1_t3D746CBB6123D4569FF4DEA60BC4240F32C6FE75* __this, const RuntimeMethod* method) 
{
	{
		float L_0 = (float)__this->___value_1;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* KeyValuePair_2_get_Key_mBD8EA7557C27E6956F2AF29DA3F7499B2F51A282_gshared_inline (KeyValuePair_2_tFC32D2507216293851350D29B64D79F950B55230* __this, const RuntimeMethod* method) 
{
	{
		RuntimeObject* L_0 = (RuntimeObject*)__this->___key_0;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* KeyValuePair_2_get_Value_mC6BD8075F9C9DDEF7B4D731E5C38EC19103988E7_gshared_inline (KeyValuePair_2_tFC32D2507216293851350D29B64D79F950B55230* __this, const RuntimeMethod* method) 
{
	{
		RuntimeObject* L_0 = (RuntimeObject*)__this->___value_1;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Nullable_1_get_HasValue_mB1F55188CDD50D6D725D41F55D2F2540CD15FB20_gshared_inline (Nullable_1_t163D49A1147F217B7BD43BE8ACC8A5CC6B846D14* __this, const RuntimeMethod* method) 
{
	{
		bool L_0 = (bool)__this->___hasValue_0;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void FacebookDelegate_1_Invoke_mBF6BA93033A7B72E328E2BA4C01FC55B970E123B_gshared_inline (FacebookDelegate_1_tC3557293F9F4D8302666EA5C4874312230B814C9* __this, RuntimeObject* ___0_result, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, RuntimeObject*, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl_1)((Il2CppObject*)__this->___method_code_6, ___0_result, reinterpret_cast<RuntimeMethod*>(__this->___method_3));
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR void Callback_1_Invoke_m8D352E98D69718836E0A8A72F8170BD075F846B5_gshared_inline (Callback_1_t244FCFB1C42CF22CE5AF6954370376FF384F449A* __this, RuntimeObject* ___0_obj, const RuntimeMethod* method) 
{
	typedef void (*FunctionPointerType) (RuntimeObject*, RuntimeObject*, const RuntimeMethod*);
	((FunctionPointerType)__this->___invoke_impl_1)((Il2CppObject*)__this->___method_code_6, ___0_obj, reinterpret_cast<RuntimeMethod*>(__this->___method_3));
}
