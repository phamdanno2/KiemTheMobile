#include "pch-cpp.hpp"

#ifndef _MSC_VER
# include <alloca.h>
#else
# include <malloc.h>
#endif


#include <limits>


template <typename R, typename T1, typename T2>
struct GenericInterfaceFuncInvoker2
{
	typedef R (*Func)(void*, T1, T2, const RuntimeMethod*);

	static inline R Invoke (const RuntimeMethod* method, RuntimeObject* obj, T1 p1, T2 p2)
	{
		VirtualInvokeData invokeData;
		il2cpp_codegen_get_generic_interface_invoke_data(method, obj, &invokeData);
		return ((Func)invokeData.methodPtr)(obj, p1, p2, invokeData.method);
	}
};

// System.Action`1<System.Collections.Generic.List`1<UnityEngine.Purchasing.Extension.ProductDescription>>
struct Action_1_tA72D33CF2F54A3A2B5EA5FC85BF59006A8BCC2BE;
// System.Action`1<UnityEngine.Purchasing.Models.GoogleRetrieveProductsFailureReason>
struct Action_1_t827DD5268A2273F9357827BCDAB0FD15F77CD462;
// System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult>
struct Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D;
// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.IEnumerable`1<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper>>
struct Action_2_t63B4C09ADDE2446D3181C3C1BF190620C33EB90D;
// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.List`1<UnityEngine.AndroidJavaObject>>
struct Action_2_tCB70C6C619E16ED17FB9F193DFE6878FEAF1C9DF;
// System.Func`2<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper,System.String>
struct Func_2_t75667D71F159AEB8D73106C0895991743541AD05;
// System.Func`2<System.Object,System.Object>
struct Func_2_tACBF5A1656250800CE861707354491F0611F6624;
// System.Collections.Generic.IEnumerable`1<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper>
struct IEnumerable_1_tB0D4C9D42D0F386807EF901F8E037DC889F14D09;
// System.Collections.Generic.IEnumerable`1<System.Object>
struct IEnumerable_1_tF95C9E01A913DD50575531C8305932628663D9E9;
// System.Collections.Generic.IEnumerable`1<System.String>
struct IEnumerable_1_t349E66EC5F09B881A8E52EE40A1AB9EC60E08E44;
// System.Collections.Generic.IList`1<UnityEngine.Purchasing.ProductDefinition>
struct IList_1_t59F64BD4671A3CFD9A6FC01A4FF2F4B732DD697D;
// System.Collections.Generic.List`1<UnityEngine.AndroidJavaObject>
struct List_1_t75A593D0EA566755481CBE3EAF0CD9CACD223EAF;
// System.Collections.Generic.List`1<System.Object>
struct List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D;
// System.Collections.Generic.List`1<UnityEngine.Purchasing.Extension.ProductDescription>
struct List_1_tC907BA3C053A12CF512BC52B3657F30C756D4B7B;
// System.Collections.Generic.List`1<System.String>
struct List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD;
// System.Collections.ObjectModel.ReadOnlyCollection`1<UnityEngine.Purchasing.ProductDefinition>
struct ReadOnlyCollection_1_tA49701F42E3782EB8804C53D26901317BAD43A9E;
// System.Byte[]
struct ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031;
// System.Delegate[]
struct DelegateU5BU5D_tC5AB7E8F745616680F337909D3A8E6C722CDF771;
// System.Object[]
struct ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918;
// System.String[]
struct StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248;
// UnityEngine.AndroidJavaClass
struct AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03;
// UnityEngine.AndroidJavaObject
struct AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0;
// System.DelegateData
struct DelegateData_t9B286B493293CD2D23A5B2B5EF0E5B1324C2B77E;
// UnityEngine.GlobalJavaObjectRef
struct GlobalJavaObjectRef_t20D8E5AAFC2EB2518FCABBF40465855E797FF0D8;
// UnityEngine.Purchasing.GoogleAcknowledgePurchaseListener
struct GoogleAcknowledgePurchaseListener_t6E473F7909F47F58F04139A8FB337B977C6A81E7;
// UnityEngine.Purchasing.Models.GoogleBillingClient
struct GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1;
// UnityEngine.Purchasing.Models.GoogleBillingResult
struct GoogleBillingResult_t745A56EF536C75D42537F287C4BF137739AC6EA4;
// UnityEngine.Purchasing.GoogleConsumeResponseListener
struct GoogleConsumeResponseListener_t554678618418EE1D7D9E4B49B8348D9239CFD8C3;
// UnityEngine.Purchasing.Models.GooglePurchase
struct GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF;
// UnityEngine.Purchasing.Models.GooglePurchaseStateEnumProvider
struct GooglePurchaseStateEnumProvider_t4F9C48DADF977FD31FFE29D767F092126332683A;
// UnityEngine.Purchasing.GooglePurchasesResponseListener
struct GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A;
// UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper
struct IAndroidJavaObjectWrapper_tC1A612D0FB5242E0B7B6FE0D545945956CFF7DF4;
// UnityEngine.Purchasing.Interfaces.IBillingClientStateListener
struct IBillingClientStateListener_t2FAE24F779EB93FF898457D3D2A30B1ED765C475;
// UnityEngine.Purchasing.Models.IGoogleBillingResult
struct IGoogleBillingResult_t2DBBCFD60D5D8E272BDE7BB5E466647D7FCE0CAB;
// UnityEngine.Purchasing.Interfaces.IGooglePurchaseUpdatedListener
struct IGooglePurchaseUpdatedListener_tB619F274A82A6BEFD04FE9922443FBFCE24587EA;
// Uniject.IUtil
struct IUtil_t57381F702008EC2AD5F50703BDD602CCA678BE66;
// System.Reflection.MethodInfo
struct MethodInfo_t;
// UnityEngine.Purchasing.Models.ProductDescriptionQuery
struct ProductDescriptionQuery_t03B36576574F6E71672313472421EE2FB8C5BFAE;
// UnityEngine.Purchasing.SkuDetailsResponseListener
struct SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4;
// System.String
struct String_t;
// System.Void
struct Void_t4861ACF8F4594C3437BB48B6E56783494B843915;
// UnityEngine.Purchasing.Models.GooglePurchase/<>c
struct U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2;

IL2CPP_EXTERN_C RuntimeClass* AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Func_2_t75667D71F159AEB8D73106C0895991743541AD05_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* GoogleAcknowledgePurchaseListener_t6E473F7909F47F58F04139A8FB337B977C6A81E7_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* GoogleConsumeResponseListener_t554678618418EE1D7D9E4B49B8348D9239CFD8C3_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* Int32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C RuntimeClass* U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var;
IL2CPP_EXTERN_C String_t* _stringLiteral001453AEE96196C60F5094DBB37BD7779972F12D;
IL2CPP_EXTERN_C String_t* _stringLiteral089C1E0B9EE6ADF5F979F43928FBC4C73BC1DC92;
IL2CPP_EXTERN_C String_t* _stringLiteral0DF1C1C5184050271D01CA0DD020721C60AE0460;
IL2CPP_EXTERN_C String_t* _stringLiteral185EA08EFC15CA94E2EEC2396C949698CC067FDB;
IL2CPP_EXTERN_C String_t* _stringLiteral24D3DE153958E752CFE514CB0421AEAA5D3AC266;
IL2CPP_EXTERN_C String_t* _stringLiteral262420555EA5B16B5A4C3D90B8838492D7CA04F9;
IL2CPP_EXTERN_C String_t* _stringLiteral27353943B75B826B09C910934BD0E73236675429;
IL2CPP_EXTERN_C String_t* _stringLiteral2778FD4BAB0076B85A6DC02C1B233BF0A7848FC0;
IL2CPP_EXTERN_C String_t* _stringLiteral2FF8A519504C6CCE22675AFBE30EAD3B2AA5F1F6;
IL2CPP_EXTERN_C String_t* _stringLiteral3261C3E11E9AB172DA0BD2010EF79C41DE23C91C;
IL2CPP_EXTERN_C String_t* _stringLiteral37DE638335775168CC634695107CD741DC14F2BC;
IL2CPP_EXTERN_C String_t* _stringLiteral450C8EF3D0450ABCD23C53730AAA221835C6A350;
IL2CPP_EXTERN_C String_t* _stringLiteral4AB2B70CADC85FDC6915309B826BAACC5034EDD4;
IL2CPP_EXTERN_C String_t* _stringLiteral4AF84F16421B44F3C9DB949CA6917212BBB501AB;
IL2CPP_EXTERN_C String_t* _stringLiteral4DA292056609E91DF87CFB0BE26ACC4860B8C273;
IL2CPP_EXTERN_C String_t* _stringLiteral5364286D453662CBFAD0610736DCAE600399206C;
IL2CPP_EXTERN_C String_t* _stringLiteral5A0370C4053F9CFA36D6BC04AF621FE2F3C3BEF3;
IL2CPP_EXTERN_C String_t* _stringLiteral5E9A8B9490715BE488FC276751AC092CED72E331;
IL2CPP_EXTERN_C String_t* _stringLiteral63C24B473E127CB6B089ACEF244BCB238A34E135;
IL2CPP_EXTERN_C String_t* _stringLiteral6DAB35E4EA4BBD5AF1473155FA1288D974D1DAD9;
IL2CPP_EXTERN_C String_t* _stringLiteral76C41506C48C50491E7B491CC16239D496B8C6CA;
IL2CPP_EXTERN_C String_t* _stringLiteral7FEA58AAF24C61EE697135803E8D03C83500C3F5;
IL2CPP_EXTERN_C String_t* _stringLiteral82E7FD0A6F1924734BD56BAAC0E26EAAB7666434;
IL2CPP_EXTERN_C String_t* _stringLiteral8BBDC2A18D5F5AE48C6CE7DD32753A2729B9B2DE;
IL2CPP_EXTERN_C String_t* _stringLiteral9303FDBBA3EA9F42A781A1107ABF8F1702BF684C;
IL2CPP_EXTERN_C String_t* _stringLiteral930CB8F6DA84828CD491A428D366B0EB14678734;
IL2CPP_EXTERN_C String_t* _stringLiteral9C65AE428D66E9596028DEE3D50639FC92DA9E83;
IL2CPP_EXTERN_C String_t* _stringLiteralA44250C90C4461C6F602B3B9DC9B873627787D3B;
IL2CPP_EXTERN_C String_t* _stringLiteralA5868C1F61F8859D84C803C66A240FA7D48F1E96;
IL2CPP_EXTERN_C String_t* _stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728;
IL2CPP_EXTERN_C String_t* _stringLiteralB68F28755D63FE386531C2F52FF6A58B380E0ECC;
IL2CPP_EXTERN_C String_t* _stringLiteralBAC49E6DFC5B4CC3310673D8D72EA1E595137E56;
IL2CPP_EXTERN_C String_t* _stringLiteralBE8C1D391821C5AE706B1E3CCB6547B999E360AA;
IL2CPP_EXTERN_C String_t* _stringLiteralC0996A36415E22F8B9021DA5470FAD41831458D9;
IL2CPP_EXTERN_C String_t* _stringLiteralC5EE8C59C90DE1E698A3010542A9B964C720ED30;
IL2CPP_EXTERN_C String_t* _stringLiteralD1BC95382E937429BD5741792056300D87684F48;
IL2CPP_EXTERN_C String_t* _stringLiteralE621E6581BCE23AE171A5EFE8813FCCCF6DC45FF;
IL2CPP_EXTERN_C String_t* _stringLiteralF169275544223C785E8F3C2E7F2BB05FB2885329;
IL2CPP_EXTERN_C String_t* _stringLiteralF3DB8521ADB71488B0A3D538F58F98B35E326552;
IL2CPP_EXTERN_C String_t* _stringLiteralF6E05D2223FEAB96CFC1CB43F18B0AC110ED5872;
IL2CPP_EXTERN_C String_t* _stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B;
IL2CPP_EXTERN_C String_t* _stringLiteralFDDA0E2D635BC7B9C335D0CAD680D884795E20A6;
IL2CPP_EXTERN_C const RuntimeMethod* AndroidJavaObjectExtensions_Enumerate_TisString_t_mACBF5A02F47B293C90E2E62AF3B5E90B471E1599_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* AndroidJavaObject_Call_TisString_t_m67FC2931E81004C3F259008314180511C3D2AF40_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Enumerable_FirstOrDefault_TisString_t_m9CA8A9DE7F8DCB619529414D42C259BDF6C05A5B_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Enumerable_Select_TisIAndroidJavaObjectWrapper_tC1A612D0FB5242E0B7B6FE0D545945956CFF7DF4_TisString_t_m762C1F9DB2640F02FBC59A9060856067CB3119E3_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Enumerable_ToList_TisString_t_m86360148F90DE6EA1A8363F38B7C2A88FD139131_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* IAndroidJavaObjectWrapper_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m1C8F17EDFAB334D7CEB13FB9A68A1B0CD4E9A77E_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* IAndroidJavaObjectWrapper_Call_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_m5F286AAE3D9A081053469736CC66C2873A0A9900_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* IAndroidJavaObjectWrapper_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m57AD306FDEC5BDE85E9715C1A0B7CFFFB7C00753_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* IAndroidJavaObjectWrapper_Call_TisString_t_m1A02C80883EF91CD3314D0856FE96818794AA538_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Nullable_1_get_HasValue_m76C9842998C91C360CE05A556EAAD8AD4A614A59_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* Nullable_1_get_Value_m253CD5D0DEEB5662FAC239342AE197DC171AE31B_RuntimeMethod_var;
IL2CPP_EXTERN_C const RuntimeMethod* U3CU3Ec_U3C_ctorU3Eb__26_0_mB666CC9852E094F68F830014EA039871BBF416BA_RuntimeMethod_var;
struct Delegate_t_marshaled_com;
struct Delegate_t_marshaled_pinvoke;

struct ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918;

IL2CPP_EXTERN_C_BEGIN
IL2CPP_EXTERN_C_END

#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif

// System.EmptyArray`1<System.Object>
struct EmptyArray_1_tDF0DD7256B115243AA6BD5558417387A734240EE  : public RuntimeObject
{
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

// System.Collections.ObjectModel.ReadOnlyCollection`1<UnityEngine.Purchasing.ProductDefinition>
struct ReadOnlyCollection_1_tA49701F42E3782EB8804C53D26901317BAD43A9E  : public RuntimeObject
{
	// System.Collections.Generic.IList`1<T> System.Collections.ObjectModel.ReadOnlyCollection`1::list
	RuntimeObject* ___list_0;
	// System.Object System.Collections.ObjectModel.ReadOnlyCollection`1::_syncRoot
	RuntimeObject* ____syncRoot_1;
};

// <PrivateImplementationDetails>
struct U3CPrivateImplementationDetailsU3E_t8D0DB3264ABFAB6DDFFE3BB44566FFCAE6765D0D  : public RuntimeObject
{
};

// UnityEngine.AndroidJavaObject
struct AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0  : public RuntimeObject
{
	// UnityEngine.GlobalJavaObjectRef UnityEngine.AndroidJavaObject::m_jobject
	GlobalJavaObjectRef_t20D8E5AAFC2EB2518FCABBF40465855E797FF0D8* ___m_jobject_1;
	// UnityEngine.GlobalJavaObjectRef UnityEngine.AndroidJavaObject::m_jclass
	GlobalJavaObjectRef_t20D8E5AAFC2EB2518FCABBF40465855E797FF0D8* ___m_jclass_2;
};

// UnityEngine.Purchasing.Models.GoogleBillingClient
struct GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1  : public RuntimeObject
{
	// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::m_BillingClient
	AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___m_BillingClient_0;
	// System.String UnityEngine.Purchasing.Models.GoogleBillingClient::m_ObfuscatedAccountId
	String_t* ___m_ObfuscatedAccountId_1;
	// System.String UnityEngine.Purchasing.Models.GoogleBillingClient::m_ObfuscatedProfileId
	String_t* ___m_ObfuscatedProfileId_2;
	// Uniject.IUtil UnityEngine.Purchasing.Models.GoogleBillingClient::m_Util
	RuntimeObject* ___m_Util_3;
};

// UnityEngine.Purchasing.Models.GoogleBillingResult
struct GoogleBillingResult_t745A56EF536C75D42537F287C4BF137739AC6EA4  : public RuntimeObject
{
	// UnityEngine.Purchasing.Models.GoogleBillingResponseCode UnityEngine.Purchasing.Models.GoogleBillingResult::<responseCode>k__BackingField
	int32_t ___U3CresponseCodeU3Ek__BackingField_0;
	// System.String UnityEngine.Purchasing.Models.GoogleBillingResult::<debugMessage>k__BackingField
	String_t* ___U3CdebugMessageU3Ek__BackingField_1;
};

// UnityEngine.Purchasing.Models.GoogleBillingStrings
struct GoogleBillingStrings_t48F0D3FE154AC4ACDCD81C88AA5A1937ECB6E085  : public RuntimeObject
{
};

// UnityEngine.Purchasing.Models.GooglePurchase
struct GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF  : public RuntimeObject
{
	// UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper UnityEngine.Purchasing.Models.GooglePurchase::<javaPurchase>k__BackingField
	RuntimeObject* ___U3CjavaPurchaseU3Ek__BackingField_0;
	// System.Int32 UnityEngine.Purchasing.Models.GooglePurchase::<purchaseState>k__BackingField
	int32_t ___U3CpurchaseStateU3Ek__BackingField_1;
	// System.Collections.Generic.List`1<System.String> UnityEngine.Purchasing.Models.GooglePurchase::<skus>k__BackingField
	List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* ___U3CskusU3Ek__BackingField_2;
	// System.String UnityEngine.Purchasing.Models.GooglePurchase::<orderId>k__BackingField
	String_t* ___U3CorderIdU3Ek__BackingField_3;
	// System.String UnityEngine.Purchasing.Models.GooglePurchase::<receipt>k__BackingField
	String_t* ___U3CreceiptU3Ek__BackingField_4;
	// System.String UnityEngine.Purchasing.Models.GooglePurchase::<signature>k__BackingField
	String_t* ___U3CsignatureU3Ek__BackingField_5;
	// System.String UnityEngine.Purchasing.Models.GooglePurchase::<originalJson>k__BackingField
	String_t* ___U3CoriginalJsonU3Ek__BackingField_6;
	// System.String UnityEngine.Purchasing.Models.GooglePurchase::<purchaseToken>k__BackingField
	String_t* ___U3CpurchaseTokenU3Ek__BackingField_7;
};

// UnityEngine.Purchasing.Models.GooglePurchaseStateEnum
struct GooglePurchaseStateEnum_tDDCC9F3F35E2DE86B6D790F7B4147DE728EACC7D  : public RuntimeObject
{
};

// UnityEngine.Purchasing.Models.GooglePurchaseStateEnumProvider
struct GooglePurchaseStateEnumProvider_t4F9C48DADF977FD31FFE29D767F092126332683A  : public RuntimeObject
{
};

// UnityEngine.Purchasing.Models.GoogleSkuTypeEnum
struct GoogleSkuTypeEnum_t9471ABA55B0D1C212ADDB21BFEEC7DEA6571335C  : public RuntimeObject
{
};

// UnityEngine.Purchasing.Models.ProductDescriptionQuery
struct ProductDescriptionQuery_t03B36576574F6E71672313472421EE2FB8C5BFAE  : public RuntimeObject
{
	// System.Collections.ObjectModel.ReadOnlyCollection`1<UnityEngine.Purchasing.ProductDefinition> UnityEngine.Purchasing.Models.ProductDescriptionQuery::products
	ReadOnlyCollection_1_tA49701F42E3782EB8804C53D26901317BAD43A9E* ___products_0;
	// System.Action`1<System.Collections.Generic.List`1<UnityEngine.Purchasing.Extension.ProductDescription>> UnityEngine.Purchasing.Models.ProductDescriptionQuery::onProductsReceived
	Action_1_tA72D33CF2F54A3A2B5EA5FC85BF59006A8BCC2BE* ___onProductsReceived_1;
	// System.Action`1<UnityEngine.Purchasing.Models.GoogleRetrieveProductsFailureReason> UnityEngine.Purchasing.Models.ProductDescriptionQuery::onRetrieveProductsFailed
	Action_1_t827DD5268A2273F9357827BCDAB0FD15F77CD462* ___onRetrieveProductsFailed_2;
};

// System.String
struct String_t  : public RuntimeObject
{
	// System.Int32 System.String::_stringLength
	int32_t ____stringLength_4;
	// System.Char System.String::_firstChar
	Il2CppChar ____firstChar_5;
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

// UnityEngine.Purchasing.Models.GooglePurchase/<>c
struct U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2  : public RuntimeObject
{
};

// System.Nullable`1<UnityEngine.Purchasing.GooglePlayProrationMode>
struct Nullable_1_t80AC45D0A85DB6A123A1C14782CD54F6ECBE3E48 
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

// UnityEngine.AndroidJavaClass
struct AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03  : public AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0
{
};

// System.Boolean
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22 
{
	// System.Boolean System.Boolean::m_value
	bool ___m_value_0;
};

// System.Char
struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17 
{
	// System.Char System.Char::m_value
	Il2CppChar ___m_value_0;
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

// System.UInt32
struct UInt32_t1833D51FFA667B18A5AA4B8D34DE284F8495D29B 
{
	// System.UInt32 System.UInt32::m_value
	uint32_t ___m_value_0;
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

// UnityEngine.AndroidJavaProxy
struct AndroidJavaProxy_tE5521F9761F7B95444B9C39FB15FDFC23F80A78D  : public RuntimeObject
{
	// UnityEngine.AndroidJavaClass UnityEngine.AndroidJavaProxy::javaInterface
	AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* ___javaInterface_0;
	// System.IntPtr UnityEngine.AndroidJavaProxy::proxyObject
	intptr_t ___proxyObject_1;
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

// UnityEngine.Purchasing.GoogleAcknowledgePurchaseListener
struct GoogleAcknowledgePurchaseListener_t6E473F7909F47F58F04139A8FB337B977C6A81E7  : public AndroidJavaProxy_tE5521F9761F7B95444B9C39FB15FDFC23F80A78D
{
	// System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult> UnityEngine.Purchasing.GoogleAcknowledgePurchaseListener::m_OnAcknowledgePurchaseResponse
	Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D* ___m_OnAcknowledgePurchaseResponse_5;
};

// UnityEngine.Purchasing.GoogleConsumeResponseListener
struct GoogleConsumeResponseListener_t554678618418EE1D7D9E4B49B8348D9239CFD8C3  : public AndroidJavaProxy_tE5521F9761F7B95444B9C39FB15FDFC23F80A78D
{
	// System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult> UnityEngine.Purchasing.GoogleConsumeResponseListener::m_OnConsumeResponse
	Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D* ___m_OnConsumeResponse_5;
};

// UnityEngine.Purchasing.GooglePurchasesResponseListener
struct GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A  : public AndroidJavaProxy_tE5521F9761F7B95444B9C39FB15FDFC23F80A78D
{
	// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.IEnumerable`1<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper>> UnityEngine.Purchasing.GooglePurchasesResponseListener::m_OnQueryPurchasesResponse
	Action_2_t63B4C09ADDE2446D3181C3C1BF190620C33EB90D* ___m_OnQueryPurchasesResponse_5;
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

// UnityEngine.Purchasing.SkuDetailsResponseListener
struct SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4  : public AndroidJavaProxy_tE5521F9761F7B95444B9C39FB15FDFC23F80A78D
{
	// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.List`1<UnityEngine.AndroidJavaObject>> UnityEngine.Purchasing.SkuDetailsResponseListener::m_OnSkuDetailsResponse
	Action_2_tCB70C6C619E16ED17FB9F193DFE6878FEAF1C9DF* ___m_OnSkuDetailsResponse_5;
	// Uniject.IUtil UnityEngine.Purchasing.SkuDetailsResponseListener::m_Util
	RuntimeObject* ___m_Util_6;
};

// System.Action`1<System.Collections.Generic.List`1<UnityEngine.Purchasing.Extension.ProductDescription>>
struct Action_1_tA72D33CF2F54A3A2B5EA5FC85BF59006A8BCC2BE  : public MulticastDelegate_t
{
};

// System.Action`1<UnityEngine.Purchasing.Models.GoogleRetrieveProductsFailureReason>
struct Action_1_t827DD5268A2273F9357827BCDAB0FD15F77CD462  : public MulticastDelegate_t
{
};

// System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult>
struct Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D  : public MulticastDelegate_t
{
};

// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.IEnumerable`1<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper>>
struct Action_2_t63B4C09ADDE2446D3181C3C1BF190620C33EB90D  : public MulticastDelegate_t
{
};

// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.List`1<UnityEngine.AndroidJavaObject>>
struct Action_2_tCB70C6C619E16ED17FB9F193DFE6878FEAF1C9DF  : public MulticastDelegate_t
{
};

// System.Func`2<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper,System.String>
struct Func_2_t75667D71F159AEB8D73106C0895991743541AD05  : public MulticastDelegate_t
{
};

// System.EmptyArray`1<System.Object>
struct EmptyArray_1_tDF0DD7256B115243AA6BD5558417387A734240EE_StaticFields
{
	// T[] System.EmptyArray`1::Value
	ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___Value_0;
};

// System.EmptyArray`1<System.Object>

// System.Collections.Generic.List`1<System.String>
struct List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD_StaticFields
{
	// T[] System.Collections.Generic.List`1::s_emptyArray
	StringU5BU5D_t7674CD946EC0CE7B3AE0BE70E6EE85F2ECD9F248* ___s_emptyArray_5;
};

// System.Collections.Generic.List`1<System.String>

// System.Collections.ObjectModel.ReadOnlyCollection`1<UnityEngine.Purchasing.ProductDefinition>

// System.Collections.ObjectModel.ReadOnlyCollection`1<UnityEngine.Purchasing.ProductDefinition>

// <PrivateImplementationDetails>

// <PrivateImplementationDetails>

// UnityEngine.AndroidJavaObject
struct AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_StaticFields
{
	// System.Boolean UnityEngine.AndroidJavaObject::enableDebugPrints
	bool ___enableDebugPrints_0;
};

// UnityEngine.AndroidJavaObject

// UnityEngine.Purchasing.Models.GoogleBillingClient

// UnityEngine.Purchasing.Models.GoogleBillingClient

// UnityEngine.Purchasing.Models.GoogleBillingResult

// UnityEngine.Purchasing.Models.GoogleBillingResult

// UnityEngine.Purchasing.Models.GoogleBillingStrings

// UnityEngine.Purchasing.Models.GoogleBillingStrings

// UnityEngine.Purchasing.Models.GooglePurchase

// UnityEngine.Purchasing.Models.GooglePurchase

// UnityEngine.Purchasing.Models.GooglePurchaseStateEnum

// UnityEngine.Purchasing.Models.GooglePurchaseStateEnum

// UnityEngine.Purchasing.Models.GooglePurchaseStateEnumProvider

// UnityEngine.Purchasing.Models.GooglePurchaseStateEnumProvider

// UnityEngine.Purchasing.Models.GoogleSkuTypeEnum

// UnityEngine.Purchasing.Models.GoogleSkuTypeEnum

// UnityEngine.Purchasing.Models.ProductDescriptionQuery

// UnityEngine.Purchasing.Models.ProductDescriptionQuery

// System.String
struct String_t_StaticFields
{
	// System.String System.String::Empty
	String_t* ___Empty_6;
};

// System.String

// UnityEngine.Purchasing.Models.GooglePurchase/<>c
struct U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_StaticFields
{
	// UnityEngine.Purchasing.Models.GooglePurchase/<>c UnityEngine.Purchasing.Models.GooglePurchase/<>c::<>9
	U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2* ___U3CU3E9_0;
	// System.Func`2<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper,System.String> UnityEngine.Purchasing.Models.GooglePurchase/<>c::<>9__26_0
	Func_2_t75667D71F159AEB8D73106C0895991743541AD05* ___U3CU3E9__26_0_1;
};

// UnityEngine.Purchasing.Models.GooglePurchase/<>c

// System.Nullable`1<UnityEngine.Purchasing.GooglePlayProrationMode>

// System.Nullable`1<UnityEngine.Purchasing.GooglePlayProrationMode>

// System.Nullable`1<System.Int32Enum>

// System.Nullable`1<System.Int32Enum>

// UnityEngine.AndroidJavaClass

// UnityEngine.AndroidJavaClass

// System.Boolean
struct Boolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_StaticFields
{
	// System.String System.Boolean::TrueString
	String_t* ___TrueString_5;
	// System.String System.Boolean::FalseString
	String_t* ___FalseString_6;
};

// System.Boolean

// System.Char
struct Char_t521A6F19B456D956AF452D926C32709DC03D6B17_StaticFields
{
	// System.Byte[] System.Char::s_categoryForLatin1
	ByteU5BU5D_tA6237BF417AE52AD70CFB4EF24A7A82613DF9031* ___s_categoryForLatin1_3;
};

// System.Char

// System.Int32

// System.Int32

// System.IntPtr
struct IntPtr_t_StaticFields
{
	// System.IntPtr System.IntPtr::Zero
	intptr_t ___Zero_1;
};

// System.IntPtr

// System.UInt32

// System.UInt32

// System.Void

// System.Void

// UnityEngine.Purchasing.GoogleAcknowledgePurchaseListener

// UnityEngine.Purchasing.GoogleAcknowledgePurchaseListener

// UnityEngine.Purchasing.GoogleConsumeResponseListener

// UnityEngine.Purchasing.GoogleConsumeResponseListener

// UnityEngine.Purchasing.GooglePurchasesResponseListener

// UnityEngine.Purchasing.GooglePurchasesResponseListener

// UnityEngine.Purchasing.SkuDetailsResponseListener

// UnityEngine.Purchasing.SkuDetailsResponseListener

// System.Action`1<System.Collections.Generic.List`1<UnityEngine.Purchasing.Extension.ProductDescription>>

// System.Action`1<System.Collections.Generic.List`1<UnityEngine.Purchasing.Extension.ProductDescription>>

// System.Action`1<UnityEngine.Purchasing.Models.GoogleRetrieveProductsFailureReason>

// System.Action`1<UnityEngine.Purchasing.Models.GoogleRetrieveProductsFailureReason>

// System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult>

// System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult>

// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.IEnumerable`1<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper>>

// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.IEnumerable`1<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper>>

// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.List`1<UnityEngine.AndroidJavaObject>>

// System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.List`1<UnityEngine.AndroidJavaObject>>

// System.Func`2<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper,System.String>

// System.Func`2<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper,System.String>
#ifdef __clang__
#pragma clang diagnostic pop
#endif
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


// ReturnType UnityEngine.AndroidJavaObject::CallStatic<System.Object>(System.String,System.Object[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* AndroidJavaObject_CallStatic_TisRuntimeObject_mCAFE27630F6092C4910E14592B050DACFCBE146F_gshared (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_methodName, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___1_args, const RuntimeMethod* method) ;
// ReturnType UnityEngine.AndroidJavaObject::Call<System.Object>(System.String,System.Object[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* AndroidJavaObject_Call_TisRuntimeObject_mA5AF1A9E0463CE91F0ACB6AC2FE0C1922B579EF7_gshared (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_methodName, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___1_args, const RuntimeMethod* method) ;
// T[] System.Array::Empty<System.Object>()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_gshared_inline (const RuntimeMethod* method) ;
// ReturnType UnityEngine.AndroidJavaObject::Call<System.Int32>(System.String,System.Object[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC_gshared (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_methodName, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___1_args, const RuntimeMethod* method) ;
// System.Boolean System.Nullable`1<System.Int32Enum>::get_HasValue()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR bool Nullable_1_get_HasValue_mB1F55188CDD50D6D725D41F55D2F2540CD15FB20_gshared_inline (Nullable_1_t163D49A1147F217B7BD43BE8ACC8A5CC6B846D14* __this, const RuntimeMethod* method) ;
// T System.Nullable`1<System.Int32Enum>::get_Value()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t Nullable_1_get_Value_m0E81D9B6F2BA5FA17AA4366C5179CD09524FCB60_gshared (Nullable_1_t163D49A1147F217B7BD43BE8ACC8A5CC6B846D14* __this, const RuntimeMethod* method) ;
// TSource System.Linq.Enumerable::FirstOrDefault<System.Object>(System.Collections.Generic.IEnumerable`1<TSource>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* Enumerable_FirstOrDefault_TisRuntimeObject_m7DE546C4F58329C905F662422736A44C50268ECD_gshared (RuntimeObject* ___0_source, const RuntimeMethod* method) ;
// System.Collections.Generic.IEnumerable`1<T> UnityEngine.Purchasing.Models.AndroidJavaObjectExtensions::Enumerate<System.Object>(UnityEngine.AndroidJavaObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* AndroidJavaObjectExtensions_Enumerate_TisRuntimeObject_mBCE5BAC766D1BE338A897668497CB1A4CDD77A2E_gshared (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_androidJavaList, const RuntimeMethod* method) ;
// System.Collections.Generic.List`1<TSource> System.Linq.Enumerable::ToList<System.Object>(System.Collections.Generic.IEnumerable`1<TSource>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR List_1_tA239CB83DE5615F348BB0507E45F490F4F7C9A8D* Enumerable_ToList_TisRuntimeObject_m6456D63764F29E6B5B2422C3DE25113577CF51EE_gshared (RuntimeObject* ___0_source, const RuntimeMethod* method) ;
// System.Void System.Func`2<System.Object,System.Object>::.ctor(System.Object,System.IntPtr)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Func_2__ctor_m7F8A01C0B02BC1D4063F4EB1E817F7A48562A398_gshared (Func_2_tACBF5A1656250800CE861707354491F0611F6624* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method) ;
// System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::Select<System.Object,System.Object>(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* Enumerable_Select_TisRuntimeObject_TisRuntimeObject_m67C538A5EBF57C4844107A8EF25DB2CAAFBAF8FB_gshared (RuntimeObject* ___0_source, Func_2_tACBF5A1656250800CE861707354491F0611F6624* ___1_selector, const RuntimeMethod* method) ;
// FieldType UnityEngine.AndroidJavaObject::GetStatic<System.Int32>(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC_gshared (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_fieldName, const RuntimeMethod* method) ;

// System.Void UnityEngine.AndroidJavaClass::.ctor(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidJavaClass__ctor_mB5466169E1151B8CC44C8FED234D79984B431389 (AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* __this, String_t* ___0_className, const RuntimeMethod* method) ;
// System.Void System.Object::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2 (RuntimeObject* __this, const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetBillingClientClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetBillingClientClass_m1E14F996196BF4138635CAB42D07135D2D830887 (const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.UnityActivity::GetCurrentActivity()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* UnityActivity_GetCurrentActivity_m4AD23C47CE2C5D5400EC5FE79E910F7E17EE7CB8 (const RuntimeMethod* method) ;
// ReturnType UnityEngine.AndroidJavaObject::CallStatic<UnityEngine.AndroidJavaObject>(System.String,System.Object[])
inline AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63 (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_methodName, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___1_args, const RuntimeMethod* method)
{
	return ((  AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* (*) (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*, const RuntimeMethod*))AndroidJavaObject_CallStatic_TisRuntimeObject_mCAFE27630F6092C4910E14592B050DACFCBE146F_gshared)(__this, ___0_methodName, ___1_args, method);
}
// ReturnType UnityEngine.AndroidJavaObject::Call<UnityEngine.AndroidJavaObject>(System.String,System.Object[])
inline AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_methodName, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___1_args, const RuntimeMethod* method)
{
	return ((  AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* (*) (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*, const RuntimeMethod*))AndroidJavaObject_Call_TisRuntimeObject_mA5AF1A9E0463CE91F0ACB6AC2FE0C1922B579EF7_gshared)(__this, ___0_methodName, ___1_args, method);
}
// T[] System.Array::Empty<System.Object>()
inline ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline (const RuntimeMethod* method)
{
	return ((  ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* (*) (const RuntimeMethod*))Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_gshared_inline)(method);
}
// System.Void UnityEngine.AndroidJavaObject::Call(System.String,System.Object[])
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void AndroidJavaObject_Call_mDEF7846E2AB1C5379069BB21049ED55A9D837B1C (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_methodName, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___1_args, const RuntimeMethod* method) ;
// ReturnType UnityEngine.AndroidJavaObject::Call<System.Int32>(System.String,System.Object[])
inline int32_t AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_methodName, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___1_args, const RuntimeMethod* method)
{
	return ((  int32_t (*) (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*, const RuntimeMethod*))AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC_gshared)(__this, ___0_methodName, ___1_args, method);
}
// System.Void UnityEngine.Purchasing.GooglePurchasesResponseListener::.ctor(System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.IEnumerable`1<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper>>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GooglePurchasesResponseListener__ctor_mEBA8C0A4FA418F9560A5A5DFA7F8D3D65F5AA9F8 (GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A* __this, Action_2_t63B4C09ADDE2446D3181C3C1BF190620C33EB90D* ___0_onQueryPurchasesResponse, const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetSkuDetailsParamClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetSkuDetailsParamClass_m23B9C69DDF3CE5E6473D8D651D3DDA07151C2185 (const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.ListExtension::ToJava(System.Collections.Generic.List`1<System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ListExtension_ToJava_mE978EBDBB715630BF3EB53D57B0DADE80E36BE44 (List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* ___0_values, const RuntimeMethod* method) ;
// System.Void UnityEngine.Purchasing.SkuDetailsResponseListener::.ctor(System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.List`1<UnityEngine.AndroidJavaObject>>,Uniject.IUtil)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void SkuDetailsResponseListener__ctor_m0A2437CE0B5E730AAB0B2EC81D53A8141D18F9B6 (SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4* __this, Action_2_tCB70C6C619E16ED17FB9F193DFE6878FEAF1C9DF* ___0_onSkuDetailsResponseAction, RuntimeObject* ___1_util, const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::MakeBillingFlowParams(UnityEngine.AndroidJavaObject,System.String,System.Nullable`1<UnityEngine.Purchasing.GooglePlayProrationMode>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GoogleBillingClient_MakeBillingFlowParams_mB97F07BB18F188942C5FEE9242A8C13F74C28037 (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_sku, String_t* ___1_oldPurchaseToken, Nullable_1_t80AC45D0A85DB6A123A1C14782CD54F6ECBE3E48 ___2_prorationMode, const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetBillingFlowParamClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetBillingFlowParamClass_m58D8DA6228AFAD52D99ECA73F12DCA7F43FD7007 (const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::SetObfuscatedAccountIdIfNeeded(UnityEngine.AndroidJavaObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GoogleBillingClient_SetObfuscatedAccountIdIfNeeded_m8F0E529640262D3F00CA1497A7E11933BCE3C2C8 (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_billingFlowParams, const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::SetObfuscatedProfileIdIfNeeded(UnityEngine.AndroidJavaObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GoogleBillingClient_SetObfuscatedProfileIdIfNeeded_m4892A481DA1DE9B548ED540F581A95CF0A917E9E (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_billingFlowParams, const RuntimeMethod* method) ;
// System.Boolean System.Nullable`1<UnityEngine.Purchasing.GooglePlayProrationMode>::get_HasValue()
inline bool Nullable_1_get_HasValue_m76C9842998C91C360CE05A556EAAD8AD4A614A59_inline (Nullable_1_t80AC45D0A85DB6A123A1C14782CD54F6ECBE3E48* __this, const RuntimeMethod* method)
{
	return ((  bool (*) (Nullable_1_t80AC45D0A85DB6A123A1C14782CD54F6ECBE3E48*, const RuntimeMethod*))Nullable_1_get_HasValue_mB1F55188CDD50D6D725D41F55D2F2540CD15FB20_gshared_inline)(__this, method);
}
// T System.Nullable`1<UnityEngine.Purchasing.GooglePlayProrationMode>::get_Value()
inline int32_t Nullable_1_get_Value_m253CD5D0DEEB5662FAC239342AE197DC171AE31B (Nullable_1_t80AC45D0A85DB6A123A1C14782CD54F6ECBE3E48* __this, const RuntimeMethod* method)
{
	return ((  int32_t (*) (Nullable_1_t80AC45D0A85DB6A123A1C14782CD54F6ECBE3E48*, const RuntimeMethod*))Nullable_1_get_Value_m0E81D9B6F2BA5FA17AA4366C5179CD09524FCB60_gshared)(__this, method);
}
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::BuildSubscriptionUpdateParams(System.String,UnityEngine.Purchasing.GooglePlayProrationMode)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GoogleBillingClient_BuildSubscriptionUpdateParams_m97A7A6F6915CCB261135B1F72679A677CB6F9033 (String_t* ___0_oldPurchaseToken, int32_t ___1_prorationMode, const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetSubscriptionUpdateParamClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetSubscriptionUpdateParamClass_mA43B88A77C88EFB159589EB987A8336571E789B5 (const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetConsumeParamsClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetConsumeParamsClass_m58C66A4B4CA41C79D27E3D1A9B5A1472FDB08E85 (const RuntimeMethod* method) ;
// System.Void UnityEngine.Purchasing.GoogleConsumeResponseListener::.ctor(System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GoogleConsumeResponseListener__ctor_m8CE0D56E7F1AA8E7CFDFCFC7050CB47DFFF2C3AB (GoogleConsumeResponseListener_t554678618418EE1D7D9E4B49B8348D9239CFD8C3* __this, Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D* ___0_onConsumeResponseAction, const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetAcknowledgePurchaseParamsClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetAcknowledgePurchaseParamsClass_m01201653BC18C4E4F35BFD3936E0DB688F734AA9 (const RuntimeMethod* method) ;
// System.Void UnityEngine.Purchasing.GoogleAcknowledgePurchaseListener::.ctor(System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GoogleAcknowledgePurchaseListener__ctor_mB509911DE8C7BEE8D023360D6E5C1BC970E94FE1 (GoogleAcknowledgePurchaseListener_t6E473F7909F47F58F04139A8FB337B977C6A81E7* __this, Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D* ___0_onAcknowledgePurchaseResponseAction, const RuntimeMethod* method) ;
// ReturnType UnityEngine.AndroidJavaObject::Call<System.String>(System.String,System.Object[])
inline String_t* AndroidJavaObject_Call_TisString_t_m67FC2931E81004C3F259008314180511C3D2AF40 (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_methodName, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* ___1_args, const RuntimeMethod* method)
{
	return ((  String_t* (*) (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*, const RuntimeMethod*))AndroidJavaObject_Call_TisRuntimeObject_mA5AF1A9E0463CE91F0ACB6AC2FE0C1922B579EF7_gshared)(__this, ___0_methodName, ___1_args, method);
}
// System.String System.String::Concat(System.String,System.String,System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B (String_t* ___0_str0, String_t* ___1_str1, String_t* ___2_str2, const RuntimeMethod* method) ;
// System.Collections.Generic.List`1<System.String> UnityEngine.Purchasing.Models.GooglePurchase::get_skus()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* GooglePurchase_get_skus_mFB5A449AA1EE9433CFE668CDE90A55B7FDEB81A4_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) ;
// TSource System.Linq.Enumerable::FirstOrDefault<System.String>(System.Collections.Generic.IEnumerable`1<TSource>)
inline String_t* Enumerable_FirstOrDefault_TisString_t_m9CA8A9DE7F8DCB619529414D42C259BDF6C05A5B (RuntimeObject* ___0_source, const RuntimeMethod* method)
{
	return ((  String_t* (*) (RuntimeObject*, const RuntimeMethod*))Enumerable_FirstOrDefault_TisRuntimeObject_m7DE546C4F58329C905F662422736A44C50268ECD_gshared)(___0_source, method);
}
// System.Collections.Generic.IEnumerable`1<T> UnityEngine.Purchasing.Models.AndroidJavaObjectExtensions::Enumerate<System.String>(UnityEngine.AndroidJavaObject)
inline RuntimeObject* AndroidJavaObjectExtensions_Enumerate_TisString_t_mACBF5A02F47B293C90E2E62AF3B5E90B471E1599 (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_androidJavaList, const RuntimeMethod* method)
{
	return ((  RuntimeObject* (*) (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0*, const RuntimeMethod*))AndroidJavaObjectExtensions_Enumerate_TisRuntimeObject_mBCE5BAC766D1BE338A897668497CB1A4CDD77A2E_gshared)(___0_androidJavaList, method);
}
// System.Collections.Generic.List`1<TSource> System.Linq.Enumerable::ToList<System.String>(System.Collections.Generic.IEnumerable`1<TSource>)
inline List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* Enumerable_ToList_TisString_t_m86360148F90DE6EA1A8363F38B7C2A88FD139131 (RuntimeObject* ___0_source, const RuntimeMethod* method)
{
	return ((  List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* (*) (RuntimeObject*, const RuntimeMethod*))Enumerable_ToList_TisRuntimeObject_m6456D63764F29E6B5B2422C3DE25113577CF51EE_gshared)(___0_source, method);
}
// System.Void System.Func`2<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper,System.String>::.ctor(System.Object,System.IntPtr)
inline void Func_2__ctor_m466D04A215C45A0E59DACB1B32B8F0C6035D6871 (Func_2_t75667D71F159AEB8D73106C0895991743541AD05* __this, RuntimeObject* ___0_object, intptr_t ___1_method, const RuntimeMethod* method)
{
	((  void (*) (Func_2_t75667D71F159AEB8D73106C0895991743541AD05*, RuntimeObject*, intptr_t, const RuntimeMethod*))Func_2__ctor_m7F8A01C0B02BC1D4063F4EB1E817F7A48562A398_gshared)(__this, ___0_object, ___1_method, method);
}
// System.Collections.Generic.IEnumerable`1<TResult> System.Linq.Enumerable::Select<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper,System.String>(System.Collections.Generic.IEnumerable`1<TSource>,System.Func`2<TSource,TResult>)
inline RuntimeObject* Enumerable_Select_TisIAndroidJavaObjectWrapper_tC1A612D0FB5242E0B7B6FE0D545945956CFF7DF4_TisString_t_m762C1F9DB2640F02FBC59A9060856067CB3119E3 (RuntimeObject* ___0_source, Func_2_t75667D71F159AEB8D73106C0895991743541AD05* ___1_selector, const RuntimeMethod* method)
{
	return ((  RuntimeObject* (*) (RuntimeObject*, Func_2_t75667D71F159AEB8D73106C0895991743541AD05*, const RuntimeMethod*))Enumerable_Select_TisRuntimeObject_TisRuntimeObject_m67C538A5EBF57C4844107A8EF25DB2CAAFBAF8FB_gshared)(___0_source, ___1_selector, method);
}
// System.String UnityEngine.Purchasing.Models.GooglePurchase::get_originalJson()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* GooglePurchase_get_originalJson_m6708011BD0AE03F2280CD86A0F07875EA578D5BA_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) ;
// System.String UnityEngine.Purchasing.Models.GooglePurchase::get_signature()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* GooglePurchase_get_signature_m72063440F5794869DB8A4DE3F56A73F4444786AC_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) ;
// System.String UnityEngine.Purchasing.Utils.GoogleReceiptEncoder::EncodeReceipt(System.String,System.String,System.Collections.Generic.List`1<System.String>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GoogleReceiptEncoder_EncodeReceipt_m17FC37EB777C0CD19B0A1345C320C17F030911D8 (String_t* ___0_purchaseOriginalJson, String_t* ___1_purchaseSignature, List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* ___2_skuDetailsJson, const RuntimeMethod* method) ;
// UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper UnityEngine.Purchasing.Models.GooglePurchase::get_javaPurchase()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* GooglePurchase_get_javaPurchase_m82A168C3FB80849E2B85BED12EE4DCA6E58CEC18_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) ;
// System.Int32 UnityEngine.Purchasing.Models.GooglePurchase::get_purchaseState()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t GooglePurchase_get_purchaseState_m25B05A607B60519FBA52843CAEF8FD8FEE0752A9_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) ;
// System.Int32 UnityEngine.Purchasing.Models.GooglePurchaseStateEnum::Purchased()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t GooglePurchaseStateEnum_Purchased_m3791A59F7885C918735F78345549C35C39E661F0 (const RuntimeMethod* method) ;
// System.Int32 UnityEngine.Purchasing.Models.GooglePurchaseStateEnum::Pending()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t GooglePurchaseStateEnum_Pending_m419C6870D3097EADAF00FF0D6FF5C486BFB13171 (const RuntimeMethod* method) ;
// System.Void UnityEngine.Purchasing.Models.GooglePurchase/<>c::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__ctor_m696F4E3E542DD5C7ADEFA41805FB149F796B836A (U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2* __this, const RuntimeMethod* method) ;
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GooglePurchaseStateEnum::GetPurchaseStateJavaObject()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GooglePurchaseStateEnum_GetPurchaseStateJavaObject_mBEFD71488906CB2105D270DACD285AFFE95C89E1 (const RuntimeMethod* method) ;
// FieldType UnityEngine.AndroidJavaObject::GetStatic<System.Int32>(System.String)
inline int32_t AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* __this, String_t* ___0_fieldName, const RuntimeMethod* method)
{
	return ((  int32_t (*) (AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0*, String_t*, const RuntimeMethod*))AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC_gshared)(__this, ___0_fieldName, method);
}
// System.Char System.String::get_Chars(System.Int32)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR Il2CppChar String_get_Chars_mC49DF0CD2D3BE7BE97B3AD9C995BE3094F8E36D3 (String_t* __this, int32_t ___0_index, const RuntimeMethod* method) ;
// System.Int32 System.String::get_Length()
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline (String_t* __this, const RuntimeMethod* method) ;
#ifdef __clang__
#pragma clang diagnostic push
#pragma clang diagnostic ignored "-Winvalid-offsetof"
#pragma clang diagnostic ignored "-Wunused-variable"
#endif
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetSkuDetailsParamClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetSkuDetailsParamClass_m23B9C69DDF3CE5E6473D8D651D3DDA07151C2185 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral0DF1C1C5184050271D01CA0DD020721C60AE0460);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return new AndroidJavaClass(k_AndroidSkuDetailsParamClassName);
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0 = (AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03*)il2cpp_codegen_object_new(AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		AndroidJavaClass__ctor_mB5466169E1151B8CC44C8FED234D79984B431389(L_0, _stringLiteral0DF1C1C5184050271D01CA0DD020721C60AE0460, NULL);
		return L_0;
	}
}
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetBillingFlowParamClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetBillingFlowParamClass_m58D8DA6228AFAD52D99ECA73F12DCA7F43FD7007 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral37DE638335775168CC634695107CD741DC14F2BC);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return new AndroidJavaClass(k_AndroidBillingFlowParamClassName);
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0 = (AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03*)il2cpp_codegen_object_new(AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		AndroidJavaClass__ctor_mB5466169E1151B8CC44C8FED234D79984B431389(L_0, _stringLiteral37DE638335775168CC634695107CD741DC14F2BC, NULL);
		return L_0;
	}
}
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetSubscriptionUpdateParamClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetSubscriptionUpdateParamClass_mA43B88A77C88EFB159589EB987A8336571E789B5 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralBAC49E6DFC5B4CC3310673D8D72EA1E595137E56);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return new AndroidJavaClass(k_AndroidSubscriptionUpdateParamClassName);
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0 = (AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03*)il2cpp_codegen_object_new(AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		AndroidJavaClass__ctor_mB5466169E1151B8CC44C8FED234D79984B431389(L_0, _stringLiteralBAC49E6DFC5B4CC3310673D8D72EA1E595137E56, NULL);
		return L_0;
	}
}
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetConsumeParamsClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetConsumeParamsClass_m58C66A4B4CA41C79D27E3D1A9B5A1472FDB08E85 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral82E7FD0A6F1924734BD56BAAC0E26EAAB7666434);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return new AndroidJavaClass(k_AndroidConsumeParamsClassName);
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0 = (AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03*)il2cpp_codegen_object_new(AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		AndroidJavaClass__ctor_mB5466169E1151B8CC44C8FED234D79984B431389(L_0, _stringLiteral82E7FD0A6F1924734BD56BAAC0E26EAAB7666434, NULL);
		return L_0;
	}
}
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetAcknowledgePurchaseParamsClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetAcknowledgePurchaseParamsClass_m01201653BC18C4E4F35BFD3936E0DB688F734AA9 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2FF8A519504C6CCE22675AFBE30EAD3B2AA5F1F6);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return new AndroidJavaClass(k_AndroidAcknowledgePurchaseParamsClassName);
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0 = (AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03*)il2cpp_codegen_object_new(AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		AndroidJavaClass__ctor_mB5466169E1151B8CC44C8FED234D79984B431389(L_0, _stringLiteral2FF8A519504C6CCE22675AFBE30EAD3B2AA5F1F6, NULL);
		return L_0;
	}
}
// UnityEngine.AndroidJavaClass UnityEngine.Purchasing.Models.GoogleBillingClient::GetBillingClientClass()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* GoogleBillingClient_GetBillingClientClass_m1E14F996196BF4138635CAB42D07135D2D830887 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4AB2B70CADC85FDC6915309B826BAACC5034EDD4);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return new AndroidJavaClass(k_AndroidBillingClientClassName);
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0 = (AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03*)il2cpp_codegen_object_new(AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		AndroidJavaClass__ctor_mB5466169E1151B8CC44C8FED234D79984B431389(L_0, _stringLiteral4AB2B70CADC85FDC6915309B826BAACC5034EDD4, NULL);
		return L_0;
	}
}
// System.Void UnityEngine.Purchasing.Models.GoogleBillingClient::.ctor(UnityEngine.Purchasing.Interfaces.IGooglePurchaseUpdatedListener,Uniject.IUtil)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GoogleBillingClient__ctor_mB2D081CC45E95911CF76F2ED1844C6F731C9E84C (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, RuntimeObject* ___0_googlePurchaseUpdatedListener, RuntimeObject* ___1_util, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral3261C3E11E9AB172DA0BD2010EF79C41DE23C91C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5A0370C4053F9CFA36D6BC04AF621FE2F3C3BEF3);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B);
		s_Il2CppMethodInitialized = true;
	}
	AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* V_0 = NULL;
	{
		// internal GoogleBillingClient(IGooglePurchaseUpdatedListener googlePurchaseUpdatedListener, IUtil util)
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		// m_Util = util;
		RuntimeObject* L_0 = ___1_util;
		__this->___m_Util_3 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___m_Util_3), (void*)L_0);
		// var builder = GetBillingClientClass().CallStatic<AndroidJavaObject>("newBuilder", UnityActivity.GetCurrentActivity());
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_1;
		L_1 = GoogleBillingClient_GetBillingClientClass_m1E14F996196BF4138635CAB42D07135D2D830887(NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_3 = L_2;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_4;
		L_4 = UnityActivity_GetCurrentActivity_m4AD23C47CE2C5D5400EC5FE79E910F7E17EE7CB8(NULL);
		NullCheck(L_3);
		ArrayElementTypeCheck (L_3, L_4);
		(L_3)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_4);
		NullCheck(L_1);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_5;
		L_5 = AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63(L_1, _stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B, L_3, AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		V_0 = L_5;
		// builder = builder.Call<AndroidJavaObject>("setListener", googlePurchaseUpdatedListener);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_6 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_7 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_8 = L_7;
		RuntimeObject* L_9 = ___0_googlePurchaseUpdatedListener;
		NullCheck(L_8);
		ArrayElementTypeCheck (L_8, L_9);
		(L_8)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_9);
		NullCheck(L_6);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_10;
		L_10 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_6, _stringLiteral3261C3E11E9AB172DA0BD2010EF79C41DE23C91C, L_8, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_10;
		// builder = builder.Call<AndroidJavaObject>("enablePendingPurchases");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_11 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_12;
		L_12 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_11);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_13;
		L_13 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_11, _stringLiteral5A0370C4053F9CFA36D6BC04AF621FE2F3C3BEF3, L_12, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_13;
		// m_BillingClient = builder.Call<AndroidJavaObject>("build");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_14 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_15;
		L_15 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_14);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_16;
		L_16 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_14, _stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728, L_15, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		__this->___m_BillingClient_0 = L_16;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___m_BillingClient_0), (void*)L_16);
		// }
		return;
	}
}
// System.Void UnityEngine.Purchasing.Models.GoogleBillingClient::StartConnection(UnityEngine.Purchasing.Interfaces.IBillingClientStateListener)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GoogleBillingClient_StartConnection_mA670096A33014C3C4D0F6D9D600000F16B725287 (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, RuntimeObject* ___0_billingClientStateListener, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralB68F28755D63FE386531C2F52FF6A58B380E0ECC);
		s_Il2CppMethodInitialized = true;
	}
	{
		// m_BillingClient.Call("startConnection", billingClientStateListener);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_0 = __this->___m_BillingClient_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2 = L_1;
		RuntimeObject* L_3 = ___0_billingClientStateListener;
		NullCheck(L_2);
		ArrayElementTypeCheck (L_2, L_3);
		(L_2)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_3);
		NullCheck(L_0);
		AndroidJavaObject_Call_mDEF7846E2AB1C5379069BB21049ED55A9D837B1C(L_0, _stringLiteralB68F28755D63FE386531C2F52FF6A58B380E0ECC, L_2, NULL);
		// }
		return;
	}
}
// UnityEngine.Purchasing.GoogleBillingConnectionState UnityEngine.Purchasing.Models.GoogleBillingClient::GetConnectionState()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t GoogleBillingClient_GetConnectionState_m83E5EDB00BC624DD2E22E0158341489A6B9E54F9 (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral089C1E0B9EE6ADF5F979F43928FBC4C73BC1DC92);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return (GoogleBillingConnectionState)m_BillingClient.Call<int>("getConnectionState");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_0 = __this->___m_BillingClient_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1;
		L_1 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_0);
		int32_t L_2;
		L_2 = AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC(L_0, _stringLiteral089C1E0B9EE6ADF5F979F43928FBC4C73BC1DC92, L_1, AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC_RuntimeMethod_var);
		return (int32_t)(L_2);
	}
}
// System.Void UnityEngine.Purchasing.Models.GoogleBillingClient::QueryPurchasesAsync(System.String,System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.IEnumerable`1<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper>>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GoogleBillingClient_QueryPurchasesAsync_m3B4FBFDC6812C4E7C4C1CBFDEB9283D098E802A6 (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, String_t* ___0_skuType, Action_2_t63B4C09ADDE2446D3181C3C1BF190620C33EB90D* ___1_onQueryPurchasesResponse, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF6E05D2223FEAB96CFC1CB43F18B0AC110ED5872);
		s_Il2CppMethodInitialized = true;
	}
	GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A* V_0 = NULL;
	{
		// var listener = new GooglePurchasesResponseListener(onQueryPurchasesResponse);
		Action_2_t63B4C09ADDE2446D3181C3C1BF190620C33EB90D* L_0 = ___1_onQueryPurchasesResponse;
		GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A* L_1 = (GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A*)il2cpp_codegen_object_new(GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A_il2cpp_TypeInfo_var);
		NullCheck(L_1);
		GooglePurchasesResponseListener__ctor_mEBA8C0A4FA418F9560A5A5DFA7F8D3D65F5AA9F8(L_1, L_0, NULL);
		V_0 = L_1;
		// m_BillingClient.Call("queryPurchasesAsync", skuType, listener);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_2 = __this->___m_BillingClient_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_3 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)2);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_4 = L_3;
		String_t* L_5 = ___0_skuType;
		NullCheck(L_4);
		ArrayElementTypeCheck (L_4, L_5);
		(L_4)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_5);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_6 = L_4;
		GooglePurchasesResponseListener_t287518A06FF048023DAFC6405BAC158CE309408A* L_7 = V_0;
		NullCheck(L_6);
		ArrayElementTypeCheck (L_6, L_7);
		(L_6)->SetAt(static_cast<il2cpp_array_size_t>(1), (RuntimeObject*)L_7);
		NullCheck(L_2);
		AndroidJavaObject_Call_mDEF7846E2AB1C5379069BB21049ED55A9D837B1C(L_2, _stringLiteralF6E05D2223FEAB96CFC1CB43F18B0AC110ED5872, L_6, NULL);
		// }
		return;
	}
}
// System.Void UnityEngine.Purchasing.Models.GoogleBillingClient::QuerySkuDetailsAsync(System.Collections.Generic.List`1<System.String>,System.String,System.Action`2<UnityEngine.Purchasing.Models.IGoogleBillingResult,System.Collections.Generic.List`1<UnityEngine.AndroidJavaObject>>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GoogleBillingClient_QuerySkuDetailsAsync_m698A3D0AA846F93955C869F7842F63594DBFCF7F (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* ___0_skus, String_t* ___1_type, Action_2_tCB70C6C619E16ED17FB9F193DFE6878FEAF1C9DF* ___2_onSkuDetailsResponseAction, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5364286D453662CBFAD0610736DCAE600399206C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral76C41506C48C50491E7B491CC16239D496B8C6CA);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral8BBDC2A18D5F5AE48C6CE7DD32753A2729B9B2DE);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B);
		s_Il2CppMethodInitialized = true;
	}
	AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* V_0 = NULL;
	SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4* V_1 = NULL;
	{
		// var skuDetailsParamsBuilder = GetSkuDetailsParamClass().CallStatic<AndroidJavaObject>("newBuilder");
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0;
		L_0 = GoogleBillingClient_GetSkuDetailsParamClass_m23B9C69DDF3CE5E6473D8D651D3DDA07151C2185(NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1;
		L_1 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_0);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_2;
		L_2 = AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63(L_0, _stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B, L_1, AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		// skuDetailsParamsBuilder = skuDetailsParamsBuilder.Call<AndroidJavaObject>("setSkusList", skus.ToJava());
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_3 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_4 = L_3;
		List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* L_5 = ___0_skus;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_6;
		L_6 = ListExtension_ToJava_mE978EBDBB715630BF3EB53D57B0DADE80E36BE44(L_5, NULL);
		NullCheck(L_4);
		ArrayElementTypeCheck (L_4, L_6);
		(L_4)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_6);
		NullCheck(L_2);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_7;
		L_7 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_2, _stringLiteral8BBDC2A18D5F5AE48C6CE7DD32753A2729B9B2DE, L_4, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		// skuDetailsParamsBuilder = skuDetailsParamsBuilder.Call<AndroidJavaObject>("setType", type);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_8 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_9 = L_8;
		String_t* L_10 = ___1_type;
		NullCheck(L_9);
		ArrayElementTypeCheck (L_9, L_10);
		(L_9)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_10);
		NullCheck(L_7);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_11;
		L_11 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_7, _stringLiteral76C41506C48C50491E7B491CC16239D496B8C6CA, L_9, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		// var skuDetailsParams = skuDetailsParamsBuilder.Call<AndroidJavaObject>("build");
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_12;
		L_12 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_11);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_13;
		L_13 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_11, _stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728, L_12, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_13;
		// var listener = new SkuDetailsResponseListener(onSkuDetailsResponseAction, m_Util);
		Action_2_tCB70C6C619E16ED17FB9F193DFE6878FEAF1C9DF* L_14 = ___2_onSkuDetailsResponseAction;
		RuntimeObject* L_15 = __this->___m_Util_3;
		SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4* L_16 = (SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4*)il2cpp_codegen_object_new(SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4_il2cpp_TypeInfo_var);
		NullCheck(L_16);
		SkuDetailsResponseListener__ctor_m0A2437CE0B5E730AAB0B2EC81D53A8141D18F9B6(L_16, L_14, L_15, NULL);
		V_1 = L_16;
		// m_BillingClient.Call("querySkuDetailsAsync", skuDetailsParams, listener);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_17 = __this->___m_BillingClient_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_18 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)2);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_19 = L_18;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_20 = V_0;
		NullCheck(L_19);
		ArrayElementTypeCheck (L_19, L_20);
		(L_19)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_20);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_21 = L_19;
		SkuDetailsResponseListener_tD6C67C90ABC799DB99209E89D362774BD9B370A4* L_22 = V_1;
		NullCheck(L_21);
		ArrayElementTypeCheck (L_21, L_22);
		(L_21)->SetAt(static_cast<il2cpp_array_size_t>(1), (RuntimeObject*)L_22);
		NullCheck(L_17);
		AndroidJavaObject_Call_mDEF7846E2AB1C5379069BB21049ED55A9D837B1C(L_17, _stringLiteral5364286D453662CBFAD0610736DCAE600399206C, L_21, NULL);
		// }
		return;
	}
}
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::LaunchBillingFlow(UnityEngine.AndroidJavaObject,System.String,System.Nullable`1<UnityEngine.Purchasing.GooglePlayProrationMode>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GoogleBillingClient_LaunchBillingFlow_mC4415F98D2442C74991C040DABF879219F0A0319 (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_sku, String_t* ___1_oldPurchaseToken, Nullable_1_t80AC45D0A85DB6A123A1C14782CD54F6ECBE3E48 ___2_prorationMode, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral185EA08EFC15CA94E2EEC2396C949698CC067FDB);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return m_BillingClient.Call<AndroidJavaObject>("launchBillingFlow", UnityActivity.GetCurrentActivity(), MakeBillingFlowParams(sku, oldPurchaseToken, prorationMode));
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_0 = __this->___m_BillingClient_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)2);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2 = L_1;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_3;
		L_3 = UnityActivity_GetCurrentActivity_m4AD23C47CE2C5D5400EC5FE79E910F7E17EE7CB8(NULL);
		NullCheck(L_2);
		ArrayElementTypeCheck (L_2, L_3);
		(L_2)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_3);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_4 = L_2;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_5 = ___0_sku;
		String_t* L_6 = ___1_oldPurchaseToken;
		Nullable_1_t80AC45D0A85DB6A123A1C14782CD54F6ECBE3E48 L_7 = ___2_prorationMode;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_8;
		L_8 = GoogleBillingClient_MakeBillingFlowParams_mB97F07BB18F188942C5FEE9242A8C13F74C28037(__this, L_5, L_6, L_7, NULL);
		NullCheck(L_4);
		ArrayElementTypeCheck (L_4, L_8);
		(L_4)->SetAt(static_cast<il2cpp_array_size_t>(1), (RuntimeObject*)L_8);
		NullCheck(L_0);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_9;
		L_9 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_0, _stringLiteral185EA08EFC15CA94E2EEC2396C949698CC067FDB, L_4, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		return L_9;
	}
}
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::MakeBillingFlowParams(UnityEngine.AndroidJavaObject,System.String,System.Nullable`1<UnityEngine.Purchasing.GooglePlayProrationMode>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GoogleBillingClient_MakeBillingFlowParams_mB97F07BB18F188942C5FEE9242A8C13F74C28037 (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_sku, String_t* ___1_oldPurchaseToken, Nullable_1_t80AC45D0A85DB6A123A1C14782CD54F6ECBE3E48 ___2_prorationMode, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Nullable_1_get_HasValue_m76C9842998C91C360CE05A556EAAD8AD4A614A59_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Nullable_1_get_Value_m253CD5D0DEEB5662FAC239342AE197DC171AE31B_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA5868C1F61F8859D84C803C66A240FA7D48F1E96);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralFDDA0E2D635BC7B9C335D0CAD680D884795E20A6);
		s_Il2CppMethodInitialized = true;
	}
	AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* V_0 = NULL;
	AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* V_1 = NULL;
	{
		// var billingFlowParams = GetBillingFlowParamClass().CallStatic<AndroidJavaObject>("newBuilder");
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0;
		L_0 = GoogleBillingClient_GetBillingFlowParamClass_m58D8DA6228AFAD52D99ECA73F12DCA7F43FD7007(NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1;
		L_1 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_0);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_2;
		L_2 = AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63(L_0, _stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B, L_1, AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		V_0 = L_2;
		// billingFlowParams = SetObfuscatedAccountIdIfNeeded(billingFlowParams);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_3 = V_0;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_4;
		L_4 = GoogleBillingClient_SetObfuscatedAccountIdIfNeeded_m8F0E529640262D3F00CA1497A7E11933BCE3C2C8(__this, L_3, NULL);
		V_0 = L_4;
		// billingFlowParams = SetObfuscatedProfileIdIfNeeded(billingFlowParams);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_5 = V_0;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_6;
		L_6 = GoogleBillingClient_SetObfuscatedProfileIdIfNeeded_m4892A481DA1DE9B548ED540F581A95CF0A917E9E(__this, L_5, NULL);
		V_0 = L_6;
		// billingFlowParams = billingFlowParams.Call<AndroidJavaObject>("setSkuDetails", sku);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_7 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_8 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_9 = L_8;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_10 = ___0_sku;
		NullCheck(L_9);
		ArrayElementTypeCheck (L_9, L_10);
		(L_9)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_10);
		NullCheck(L_7);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_11;
		L_11 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_7, _stringLiteralA5868C1F61F8859D84C803C66A240FA7D48F1E96, L_9, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_11;
		// if (oldPurchaseToken != null && prorationMode != null)
		String_t* L_12 = ___1_oldPurchaseToken;
		if (!L_12)
		{
			goto IL_006b;
		}
	}
	{
		bool L_13;
		L_13 = Nullable_1_get_HasValue_m76C9842998C91C360CE05A556EAAD8AD4A614A59_inline((&___2_prorationMode), Nullable_1_get_HasValue_m76C9842998C91C360CE05A556EAAD8AD4A614A59_RuntimeMethod_var);
		if (!L_13)
		{
			goto IL_006b;
		}
	}
	{
		// var subscriptionUpdateParams = BuildSubscriptionUpdateParams(oldPurchaseToken, prorationMode.Value);
		String_t* L_14 = ___1_oldPurchaseToken;
		int32_t L_15;
		L_15 = Nullable_1_get_Value_m253CD5D0DEEB5662FAC239342AE197DC171AE31B((&___2_prorationMode), Nullable_1_get_Value_m253CD5D0DEEB5662FAC239342AE197DC171AE31B_RuntimeMethod_var);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_16;
		L_16 = GoogleBillingClient_BuildSubscriptionUpdateParams_m97A7A6F6915CCB261135B1F72679A677CB6F9033(L_14, L_15, NULL);
		V_1 = L_16;
		// billingFlowParams = billingFlowParams.Call<AndroidJavaObject>("setSubscriptionUpdateParams", subscriptionUpdateParams);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_17 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_18 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_19 = L_18;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_20 = V_1;
		NullCheck(L_19);
		ArrayElementTypeCheck (L_19, L_20);
		(L_19)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_20);
		NullCheck(L_17);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_21;
		L_21 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_17, _stringLiteralFDDA0E2D635BC7B9C335D0CAD680D884795E20A6, L_19, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_21;
	}

IL_006b:
	{
		// billingFlowParams = billingFlowParams.Call<AndroidJavaObject>("build");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_22 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_23;
		L_23 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_22);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_24;
		L_24 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_22, _stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728, L_23, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_24;
		// return billingFlowParams;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_25 = V_0;
		return L_25;
	}
}
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::BuildSubscriptionUpdateParams(System.String,UnityEngine.Purchasing.GooglePlayProrationMode)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GoogleBillingClient_BuildSubscriptionUpdateParams_m97A7A6F6915CCB261135B1F72679A677CB6F9033 (String_t* ___0_oldPurchaseToken, int32_t ___1_prorationMode, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Int32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral930CB8F6DA84828CD491A428D366B0EB14678734);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralBE8C1D391821C5AE706B1E3CCB6547B999E360AA);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B);
		s_Il2CppMethodInitialized = true;
	}
	{
		// var subscriptionUpdateParams = GetSubscriptionUpdateParamClass().CallStatic<AndroidJavaObject>("newBuilder");
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0;
		L_0 = GoogleBillingClient_GetSubscriptionUpdateParamClass_mA43B88A77C88EFB159589EB987A8336571E789B5(NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1;
		L_1 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_0);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_2;
		L_2 = AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63(L_0, _stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B, L_1, AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		// subscriptionUpdateParams = subscriptionUpdateParams.Call<AndroidJavaObject>("setReplaceSkusProrationMode", (int)prorationMode);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_3 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_4 = L_3;
		int32_t L_5 = ___1_prorationMode;
		int32_t L_6 = ((int32_t)L_5);
		RuntimeObject* L_7 = Box(Int32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_il2cpp_TypeInfo_var, &L_6);
		NullCheck(L_4);
		ArrayElementTypeCheck (L_4, L_7);
		(L_4)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_7);
		NullCheck(L_2);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_8;
		L_8 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_2, _stringLiteral930CB8F6DA84828CD491A428D366B0EB14678734, L_4, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		// subscriptionUpdateParams = subscriptionUpdateParams.Call<AndroidJavaObject>("setOldSkuPurchaseToken", oldPurchaseToken);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_9 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_10 = L_9;
		String_t* L_11 = ___0_oldPurchaseToken;
		NullCheck(L_10);
		ArrayElementTypeCheck (L_10, L_11);
		(L_10)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_11);
		NullCheck(L_8);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_12;
		L_12 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_8, _stringLiteralBE8C1D391821C5AE706B1E3CCB6547B999E360AA, L_10, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		// subscriptionUpdateParams = subscriptionUpdateParams.Call<AndroidJavaObject>("build");
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_13;
		L_13 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_12);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_14;
		L_14 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_12, _stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728, L_13, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		// return subscriptionUpdateParams;
		return L_14;
	}
}
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::SetObfuscatedProfileIdIfNeeded(UnityEngine.AndroidJavaObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GoogleBillingClient_SetObfuscatedProfileIdIfNeeded_m4892A481DA1DE9B548ED540F581A95CF0A917E9E (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_billingFlowParams, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4AF84F16421B44F3C9DB949CA6917212BBB501AB);
		s_Il2CppMethodInitialized = true;
	}
	{
		// if (m_ObfuscatedProfileId != null)
		String_t* L_0 = __this->___m_ObfuscatedProfileId_2;
		if (!L_0)
		{
			goto IL_0024;
		}
	}
	{
		// billingFlowParams = billingFlowParams.Call<AndroidJavaObject>("setObfuscatedProfileId", m_ObfuscatedProfileId);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_1 = ___0_billingFlowParams;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_3 = L_2;
		String_t* L_4 = __this->___m_ObfuscatedProfileId_2;
		NullCheck(L_3);
		ArrayElementTypeCheck (L_3, L_4);
		(L_3)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_4);
		NullCheck(L_1);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_5;
		L_5 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_1, _stringLiteral4AF84F16421B44F3C9DB949CA6917212BBB501AB, L_3, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		___0_billingFlowParams = L_5;
	}

IL_0024:
	{
		// return billingFlowParams;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_6 = ___0_billingFlowParams;
		return L_6;
	}
}
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GoogleBillingClient::SetObfuscatedAccountIdIfNeeded(UnityEngine.AndroidJavaObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GoogleBillingClient_SetObfuscatedAccountIdIfNeeded_m8F0E529640262D3F00CA1497A7E11933BCE3C2C8 (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_billingFlowParams, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral27353943B75B826B09C910934BD0E73236675429);
		s_Il2CppMethodInitialized = true;
	}
	{
		// if (m_ObfuscatedAccountId != null)
		String_t* L_0 = __this->___m_ObfuscatedAccountId_1;
		if (!L_0)
		{
			goto IL_0024;
		}
	}
	{
		// billingFlowParams = billingFlowParams.Call<AndroidJavaObject>("setObfuscatedAccountId", m_ObfuscatedAccountId);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_1 = ___0_billingFlowParams;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_3 = L_2;
		String_t* L_4 = __this->___m_ObfuscatedAccountId_1;
		NullCheck(L_3);
		ArrayElementTypeCheck (L_3, L_4);
		(L_3)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_4);
		NullCheck(L_1);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_5;
		L_5 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_1, _stringLiteral27353943B75B826B09C910934BD0E73236675429, L_3, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		___0_billingFlowParams = L_5;
	}

IL_0024:
	{
		// return billingFlowParams;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_6 = ___0_billingFlowParams;
		return L_6;
	}
}
// System.Void UnityEngine.Purchasing.Models.GoogleBillingClient::ConsumeAsync(System.String,System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GoogleBillingClient_ConsumeAsync_m20CCB9AB464691E6DAE77D0C0B6011AC2554FCDD (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, String_t* ___0_purchaseToken, Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D* ___1_onConsume, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&GoogleConsumeResponseListener_t554678618418EE1D7D9E4B49B8348D9239CFD8C3_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral24D3DE153958E752CFE514CB0421AEAA5D3AC266);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral5E9A8B9490715BE488FC276751AC092CED72E331);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B);
		s_Il2CppMethodInitialized = true;
	}
	AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* V_0 = NULL;
	{
		// var consumeParams = GetConsumeParamsClass().CallStatic<AndroidJavaObject>("newBuilder");
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0;
		L_0 = GoogleBillingClient_GetConsumeParamsClass_m58C66A4B4CA41C79D27E3D1A9B5A1472FDB08E85(NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1;
		L_1 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_0);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_2;
		L_2 = AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63(L_0, _stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B, L_1, AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		V_0 = L_2;
		// consumeParams = consumeParams.Call<AndroidJavaObject>("setPurchaseToken", purchaseToken);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_3 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_4 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_5 = L_4;
		String_t* L_6 = ___0_purchaseToken;
		NullCheck(L_5);
		ArrayElementTypeCheck (L_5, L_6);
		(L_5)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_6);
		NullCheck(L_3);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_7;
		L_7 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_3, _stringLiteral24D3DE153958E752CFE514CB0421AEAA5D3AC266, L_5, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_7;
		// consumeParams = consumeParams.Call<AndroidJavaObject>("build");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_8 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_9;
		L_9 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_8);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_10;
		L_10 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_8, _stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728, L_9, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_10;
		// m_BillingClient.Call("consumeAsync", consumeParams, new GoogleConsumeResponseListener(onConsume));
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_11 = __this->___m_BillingClient_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_12 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)2);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_13 = L_12;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_14 = V_0;
		NullCheck(L_13);
		ArrayElementTypeCheck (L_13, L_14);
		(L_13)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_14);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_15 = L_13;
		Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D* L_16 = ___1_onConsume;
		GoogleConsumeResponseListener_t554678618418EE1D7D9E4B49B8348D9239CFD8C3* L_17 = (GoogleConsumeResponseListener_t554678618418EE1D7D9E4B49B8348D9239CFD8C3*)il2cpp_codegen_object_new(GoogleConsumeResponseListener_t554678618418EE1D7D9E4B49B8348D9239CFD8C3_il2cpp_TypeInfo_var);
		NullCheck(L_17);
		GoogleConsumeResponseListener__ctor_m8CE0D56E7F1AA8E7CFDFCFC7050CB47DFFF2C3AB(L_17, L_16, NULL);
		NullCheck(L_15);
		ArrayElementTypeCheck (L_15, L_17);
		(L_15)->SetAt(static_cast<il2cpp_array_size_t>(1), (RuntimeObject*)L_17);
		NullCheck(L_11);
		AndroidJavaObject_Call_mDEF7846E2AB1C5379069BB21049ED55A9D837B1C(L_11, _stringLiteral5E9A8B9490715BE488FC276751AC092CED72E331, L_15, NULL);
		// }
		return;
	}
}
// System.Void UnityEngine.Purchasing.Models.GoogleBillingClient::AcknowledgePurchase(System.String,System.Action`1<UnityEngine.Purchasing.Models.IGoogleBillingResult>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GoogleBillingClient_AcknowledgePurchase_m019D45043AC9BD3B1FC8B20187AA25A78188F9CD (GoogleBillingClient_t4165D93CF6ECC22C599E7DDDE8E3FA54879690C1* __this, String_t* ___0_purchaseToken, Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D* ___1_onAcknowledge, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&GoogleAcknowledgePurchaseListener_t6E473F7909F47F58F04139A8FB337B977C6A81E7_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral24D3DE153958E752CFE514CB0421AEAA5D3AC266);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral7FEA58AAF24C61EE697135803E8D03C83500C3F5);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B);
		s_Il2CppMethodInitialized = true;
	}
	AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* V_0 = NULL;
	{
		// var acknowledgePurchaseParams = GetAcknowledgePurchaseParamsClass().CallStatic<AndroidJavaObject>("newBuilder");
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0;
		L_0 = GoogleBillingClient_GetAcknowledgePurchaseParamsClass_m01201653BC18C4E4F35BFD3936E0DB688F734AA9(NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1;
		L_1 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_0);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_2;
		L_2 = AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63(L_0, _stringLiteralF89E2B8AEFEFD95D439A48449E4C25ACB8455C5B, L_1, AndroidJavaObject_CallStatic_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m398EA96C1DE1BB885F2B1DD0E00E8BBA86B49E63_RuntimeMethod_var);
		V_0 = L_2;
		// acknowledgePurchaseParams = acknowledgePurchaseParams.Call<AndroidJavaObject>("setPurchaseToken", purchaseToken);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_3 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_4 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)1);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_5 = L_4;
		String_t* L_6 = ___0_purchaseToken;
		NullCheck(L_5);
		ArrayElementTypeCheck (L_5, L_6);
		(L_5)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_6);
		NullCheck(L_3);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_7;
		L_7 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_3, _stringLiteral24D3DE153958E752CFE514CB0421AEAA5D3AC266, L_5, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_7;
		// acknowledgePurchaseParams = acknowledgePurchaseParams.Call<AndroidJavaObject>("build");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_8 = V_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_9;
		L_9 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_8);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_10;
		L_10 = AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D(L_8, _stringLiteralA733C7FC19A8317471D21AD091D1A9A6F973A728, L_9, AndroidJavaObject_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m020246E0988293B6126B690BD6CE4D894276AA3D_RuntimeMethod_var);
		V_0 = L_10;
		// m_BillingClient.Call("acknowledgePurchase", acknowledgePurchaseParams, new GoogleAcknowledgePurchaseListener(onAcknowledge));
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_11 = __this->___m_BillingClient_0;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_12 = (ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918*)SZArrayNew(ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918_il2cpp_TypeInfo_var, (uint32_t)2);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_13 = L_12;
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_14 = V_0;
		NullCheck(L_13);
		ArrayElementTypeCheck (L_13, L_14);
		(L_13)->SetAt(static_cast<il2cpp_array_size_t>(0), (RuntimeObject*)L_14);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_15 = L_13;
		Action_1_tC0F6621EB53EDD3D0A48E63AC5F65F60E5FA319D* L_16 = ___1_onAcknowledge;
		GoogleAcknowledgePurchaseListener_t6E473F7909F47F58F04139A8FB337B977C6A81E7* L_17 = (GoogleAcknowledgePurchaseListener_t6E473F7909F47F58F04139A8FB337B977C6A81E7*)il2cpp_codegen_object_new(GoogleAcknowledgePurchaseListener_t6E473F7909F47F58F04139A8FB337B977C6A81E7_il2cpp_TypeInfo_var);
		NullCheck(L_17);
		GoogleAcknowledgePurchaseListener__ctor_mB509911DE8C7BEE8D023360D6E5C1BC970E94FE1(L_17, L_16, NULL);
		NullCheck(L_15);
		ArrayElementTypeCheck (L_15, L_17);
		(L_15)->SetAt(static_cast<il2cpp_array_size_t>(1), (RuntimeObject*)L_17);
		NullCheck(L_11);
		AndroidJavaObject_Call_mDEF7846E2AB1C5379069BB21049ED55A9D837B1C(L_11, _stringLiteral7FEA58AAF24C61EE697135803E8D03C83500C3F5, L_15, NULL);
		// }
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
// UnityEngine.Purchasing.Models.GoogleBillingResponseCode UnityEngine.Purchasing.Models.GoogleBillingResult::get_responseCode()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t GoogleBillingResult_get_responseCode_m41C985D833239D91A30D60B5E0F78F63D40FCEDD (GoogleBillingResult_t745A56EF536C75D42537F287C4BF137739AC6EA4* __this, const RuntimeMethod* method) 
{
	{
		// public GoogleBillingResponseCode responseCode { get; }
		int32_t L_0 = __this->___U3CresponseCodeU3Ek__BackingField_0;
		return L_0;
	}
}
// System.String UnityEngine.Purchasing.Models.GoogleBillingResult::get_debugMessage()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GoogleBillingResult_get_debugMessage_mCBC8D3C771085DE43CFBF8A67CC21FDE52684CEA (GoogleBillingResult_t745A56EF536C75D42537F287C4BF137739AC6EA4* __this, const RuntimeMethod* method) 
{
	{
		// public string debugMessage { get; }
		String_t* L_0 = __this->___U3CdebugMessageU3Ek__BackingField_1;
		return L_0;
	}
}
// System.Void UnityEngine.Purchasing.Models.GoogleBillingResult::.ctor(UnityEngine.AndroidJavaObject)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GoogleBillingResult__ctor_mA4E4F80D1EF645AC6E72981FA7F7E141F6601377 (GoogleBillingResult_t745A56EF536C75D42537F287C4BF137739AC6EA4* __this, AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* ___0_billingResult, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_Call_TisString_t_m67FC2931E81004C3F259008314180511C3D2AF40_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral4DA292056609E91DF87CFB0BE26ACC4860B8C273);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral63C24B473E127CB6B089ACEF244BCB238A34E135);
		s_Il2CppMethodInitialized = true;
	}
	{
		// internal GoogleBillingResult(AndroidJavaObject billingResult)
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		// if (billingResult != null)
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_0 = ___0_billingResult;
		if (!L_0)
		{
			goto IL_0035;
		}
	}
	{
		// responseCode = (GoogleBillingResponseCode)billingResult.Call<int>("getResponseCode");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_1 = ___0_billingResult;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2;
		L_2 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_1);
		int32_t L_3;
		L_3 = AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC(L_1, _stringLiteral4DA292056609E91DF87CFB0BE26ACC4860B8C273, L_2, AndroidJavaObject_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_mDC5FD095AFC55DFE596907E5B055B5774DA5B5AC_RuntimeMethod_var);
		__this->___U3CresponseCodeU3Ek__BackingField_0 = L_3;
		// debugMessage = billingResult.Call<string>("getDebugMessage");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_4 = ___0_billingResult;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_5;
		L_5 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_4);
		String_t* L_6;
		L_6 = AndroidJavaObject_Call_TisString_t_m67FC2931E81004C3F259008314180511C3D2AF40(L_4, _stringLiteral63C24B473E127CB6B089ACEF244BCB238A34E135, L_5, AndroidJavaObject_Call_TisString_t_m67FC2931E81004C3F259008314180511C3D2AF40_RuntimeMethod_var);
		__this->___U3CdebugMessageU3Ek__BackingField_1 = L_6;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CdebugMessageU3Ek__BackingField_1), (void*)L_6);
	}

IL_0035:
	{
		// }
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
// System.String UnityEngine.Purchasing.Models.GoogleBillingStrings::getWarningMessageMoreThanOneSkuFound(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GoogleBillingStrings_getWarningMessageMoreThanOneSkuFound_m7537B087FDB054238E02B64C5998D2FD4D4C3FD1 (String_t* ___0_sku, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral2778FD4BAB0076B85A6DC02C1B233BF0A7848FC0);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral9C65AE428D66E9596028DEE3D50639FC92DA9E83);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return "More than one SKU found when purchasing SKU " + sku + ". Please verify your Google Play Store in-app product settings.";
		String_t* L_0 = ___0_sku;
		String_t* L_1;
		L_1 = String_Concat_m8855A6DE10F84DA7F4EC113CADDB59873A25573B(_stringLiteral2778FD4BAB0076B85A6DC02C1B233BF0A7848FC0, L_0, _stringLiteral9C65AE428D66E9596028DEE3D50639FC92DA9E83, NULL);
		return L_1;
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
// UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper UnityEngine.Purchasing.Models.GooglePurchase::get_javaPurchase()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR RuntimeObject* GooglePurchase_get_javaPurchase_m82A168C3FB80849E2B85BED12EE4DCA6E58CEC18 (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public IAndroidJavaObjectWrapper javaPurchase { get; }
		RuntimeObject* L_0 = __this->___U3CjavaPurchaseU3Ek__BackingField_0;
		return L_0;
	}
}
// System.Int32 UnityEngine.Purchasing.Models.GooglePurchase::get_purchaseState()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t GooglePurchase_get_purchaseState_m25B05A607B60519FBA52843CAEF8FD8FEE0752A9 (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public int purchaseState { get; }
		int32_t L_0 = __this->___U3CpurchaseStateU3Ek__BackingField_1;
		return L_0;
	}
}
// System.Collections.Generic.List`1<System.String> UnityEngine.Purchasing.Models.GooglePurchase::get_skus()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* GooglePurchase_get_skus_mFB5A449AA1EE9433CFE668CDE90A55B7FDEB81A4 (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public List<string> skus { get; }
		List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* L_0 = __this->___U3CskusU3Ek__BackingField_2;
		return L_0;
	}
}
// System.String UnityEngine.Purchasing.Models.GooglePurchase::get_receipt()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GooglePurchase_get_receipt_mB7E801F89576DA092E7A95DC41037E0FDC9E026A (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public string receipt { get; }
		String_t* L_0 = __this->___U3CreceiptU3Ek__BackingField_4;
		return L_0;
	}
}
// System.String UnityEngine.Purchasing.Models.GooglePurchase::get_signature()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GooglePurchase_get_signature_m72063440F5794869DB8A4DE3F56A73F4444786AC (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public string signature { get; }
		String_t* L_0 = __this->___U3CsignatureU3Ek__BackingField_5;
		return L_0;
	}
}
// System.String UnityEngine.Purchasing.Models.GooglePurchase::get_originalJson()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GooglePurchase_get_originalJson_m6708011BD0AE03F2280CD86A0F07875EA578D5BA (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public string originalJson { get; }
		String_t* L_0 = __this->___U3CoriginalJsonU3Ek__BackingField_6;
		return L_0;
	}
}
// System.String UnityEngine.Purchasing.Models.GooglePurchase::get_purchaseToken()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GooglePurchase_get_purchaseToken_mEAE44EFF7955BD8A92147AC6A5B8A70A6541EDE7 (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public string purchaseToken { get; }
		String_t* L_0 = __this->___U3CpurchaseTokenU3Ek__BackingField_7;
		return L_0;
	}
}
// System.String UnityEngine.Purchasing.Models.GooglePurchase::get_sku()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GooglePurchase_get_sku_m58FFD30FBFB7CD671E343E2C61CAE80582C9EB94 (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Enumerable_FirstOrDefault_TisString_t_m9CA8A9DE7F8DCB619529414D42C259BDF6C05A5B_RuntimeMethod_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		// public string? sku => skus.FirstOrDefault();
		List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* L_0;
		L_0 = GooglePurchase_get_skus_mFB5A449AA1EE9433CFE668CDE90A55B7FDEB81A4_inline(__this, NULL);
		String_t* L_1;
		L_1 = Enumerable_FirstOrDefault_TisString_t_m9CA8A9DE7F8DCB619529414D42C259BDF6C05A5B(L_0, Enumerable_FirstOrDefault_TisString_t_m9CA8A9DE7F8DCB619529414D42C259BDF6C05A5B_RuntimeMethod_var);
		return L_1;
	}
}
// System.Void UnityEngine.Purchasing.Models.GooglePurchase::.ctor(UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper,System.Collections.Generic.IEnumerable`1<UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GooglePurchase__ctor_m1F2F9ED18508F3AC24E4C7364307ABED26EB7CED (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, RuntimeObject* ___0_purchase, RuntimeObject* ___1_skuDetails, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObjectExtensions_Enumerate_TisString_t_mACBF5A02F47B293C90E2E62AF3B5E90B471E1599_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Enumerable_Select_TisIAndroidJavaObjectWrapper_tC1A612D0FB5242E0B7B6FE0D545945956CFF7DF4_TisString_t_m762C1F9DB2640F02FBC59A9060856067CB3119E3_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Enumerable_ToList_TisString_t_m86360148F90DE6EA1A8363F38B7C2A88FD139131_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Func_2_t75667D71F159AEB8D73106C0895991743541AD05_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidJavaObjectWrapper_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m1C8F17EDFAB334D7CEB13FB9A68A1B0CD4E9A77E_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidJavaObjectWrapper_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m57AD306FDEC5BDE85E9715C1A0B7CFFFB7C00753_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidJavaObjectWrapper_Call_TisString_t_m1A02C80883EF91CD3314D0856FE96818794AA538_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_U3C_ctorU3Eb__26_0_mB666CC9852E094F68F830014EA039871BBF416BA_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral262420555EA5B16B5A4C3D90B8838492D7CA04F9);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral450C8EF3D0450ABCD23C53730AAA221835C6A350);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral9303FDBBA3EA9F42A781A1107ABF8F1702BF684C);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC0996A36415E22F8B9021DA5470FAD41831458D9);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralD1BC95382E937429BD5741792056300D87684F48);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF169275544223C785E8F3C2E7F2BB05FB2885329);
		s_Il2CppMethodInitialized = true;
	}
	List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* V_0 = NULL;
	Func_2_t75667D71F159AEB8D73106C0895991743541AD05* G_B2_0 = NULL;
	RuntimeObject* G_B2_1 = NULL;
	Func_2_t75667D71F159AEB8D73106C0895991743541AD05* G_B1_0 = NULL;
	RuntimeObject* G_B1_1 = NULL;
	{
		// internal GooglePurchase(IAndroidJavaObjectWrapper purchase, IEnumerable<IAndroidJavaObjectWrapper> skuDetails)
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		// javaPurchase = purchase;
		RuntimeObject* L_0 = ___0_purchase;
		__this->___U3CjavaPurchaseU3Ek__BackingField_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CjavaPurchaseU3Ek__BackingField_0), (void*)L_0);
		// purchaseState = purchase.Call<int>("getPurchaseState");
		RuntimeObject* L_1 = ___0_purchase;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2;
		L_2 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_1);
		int32_t L_3;
		L_3 = GenericInterfaceFuncInvoker2< int32_t, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* >::Invoke(IAndroidJavaObjectWrapper_Call_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m57AD306FDEC5BDE85E9715C1A0B7CFFFB7C00753_RuntimeMethod_var, L_1, _stringLiteralC0996A36415E22F8B9021DA5470FAD41831458D9, L_2);
		__this->___U3CpurchaseStateU3Ek__BackingField_1 = L_3;
		// skus = purchase.Call<AndroidJavaObject>("getSkus").Enumerate<string>().ToList();
		RuntimeObject* L_4 = ___0_purchase;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_5;
		L_5 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_4);
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_6;
		L_6 = GenericInterfaceFuncInvoker2< AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* >::Invoke(IAndroidJavaObjectWrapper_Call_TisAndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0_m1C8F17EDFAB334D7CEB13FB9A68A1B0CD4E9A77E_RuntimeMethod_var, L_4, _stringLiteral262420555EA5B16B5A4C3D90B8838492D7CA04F9, L_5);
		RuntimeObject* L_7;
		L_7 = AndroidJavaObjectExtensions_Enumerate_TisString_t_mACBF5A02F47B293C90E2E62AF3B5E90B471E1599(L_6, AndroidJavaObjectExtensions_Enumerate_TisString_t_mACBF5A02F47B293C90E2E62AF3B5E90B471E1599_RuntimeMethod_var);
		List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* L_8;
		L_8 = Enumerable_ToList_TisString_t_m86360148F90DE6EA1A8363F38B7C2A88FD139131(L_7, Enumerable_ToList_TisString_t_m86360148F90DE6EA1A8363F38B7C2A88FD139131_RuntimeMethod_var);
		__this->___U3CskusU3Ek__BackingField_2 = L_8;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CskusU3Ek__BackingField_2), (void*)L_8);
		// orderId = purchase.Call<string>("getOrderId");
		RuntimeObject* L_9 = ___0_purchase;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_10;
		L_10 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_9);
		String_t* L_11;
		L_11 = GenericInterfaceFuncInvoker2< String_t*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* >::Invoke(IAndroidJavaObjectWrapper_Call_TisString_t_m1A02C80883EF91CD3314D0856FE96818794AA538_RuntimeMethod_var, L_9, _stringLiteralD1BC95382E937429BD5741792056300D87684F48, L_10);
		__this->___U3CorderIdU3Ek__BackingField_3 = L_11;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CorderIdU3Ek__BackingField_3), (void*)L_11);
		// originalJson = purchase.Call<string>("getOriginalJson");
		RuntimeObject* L_12 = ___0_purchase;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_13;
		L_13 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_12);
		String_t* L_14;
		L_14 = GenericInterfaceFuncInvoker2< String_t*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* >::Invoke(IAndroidJavaObjectWrapper_Call_TisString_t_m1A02C80883EF91CD3314D0856FE96818794AA538_RuntimeMethod_var, L_12, _stringLiteral9303FDBBA3EA9F42A781A1107ABF8F1702BF684C, L_13);
		__this->___U3CoriginalJsonU3Ek__BackingField_6 = L_14;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CoriginalJsonU3Ek__BackingField_6), (void*)L_14);
		// signature = purchase.Call<string>("getSignature");
		RuntimeObject* L_15 = ___0_purchase;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_16;
		L_16 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_15);
		String_t* L_17;
		L_17 = GenericInterfaceFuncInvoker2< String_t*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* >::Invoke(IAndroidJavaObjectWrapper_Call_TisString_t_m1A02C80883EF91CD3314D0856FE96818794AA538_RuntimeMethod_var, L_15, _stringLiteral450C8EF3D0450ABCD23C53730AAA221835C6A350, L_16);
		__this->___U3CsignatureU3Ek__BackingField_5 = L_17;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CsignatureU3Ek__BackingField_5), (void*)L_17);
		// purchaseToken = purchase.Call<string>("getPurchaseToken");
		RuntimeObject* L_18 = ___0_purchase;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_19;
		L_19 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_18);
		String_t* L_20;
		L_20 = GenericInterfaceFuncInvoker2< String_t*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* >::Invoke(IAndroidJavaObjectWrapper_Call_TisString_t_m1A02C80883EF91CD3314D0856FE96818794AA538_RuntimeMethod_var, L_18, _stringLiteralF169275544223C785E8F3C2E7F2BB05FB2885329, L_19);
		__this->___U3CpurchaseTokenU3Ek__BackingField_7 = L_20;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CpurchaseTokenU3Ek__BackingField_7), (void*)L_20);
		// var skuDetailsJson = skuDetails.Select(skuDetail => skuDetail.Call<string>("getOriginalJson")).ToList();
		RuntimeObject* L_21 = ___1_skuDetails;
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var);
		Func_2_t75667D71F159AEB8D73106C0895991743541AD05* L_22 = ((U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var))->___U3CU3E9__26_0_1;
		Func_2_t75667D71F159AEB8D73106C0895991743541AD05* L_23 = L_22;
		G_B1_0 = L_23;
		G_B1_1 = L_21;
		if (L_23)
		{
			G_B2_0 = L_23;
			G_B2_1 = L_21;
			goto IL_00bb;
		}
	}
	{
		il2cpp_codegen_runtime_class_init_inline(U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var);
		U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2* L_24 = ((U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var))->___U3CU3E9_0;
		Func_2_t75667D71F159AEB8D73106C0895991743541AD05* L_25 = (Func_2_t75667D71F159AEB8D73106C0895991743541AD05*)il2cpp_codegen_object_new(Func_2_t75667D71F159AEB8D73106C0895991743541AD05_il2cpp_TypeInfo_var);
		NullCheck(L_25);
		Func_2__ctor_m466D04A215C45A0E59DACB1B32B8F0C6035D6871(L_25, L_24, (intptr_t)((void*)U3CU3Ec_U3C_ctorU3Eb__26_0_mB666CC9852E094F68F830014EA039871BBF416BA_RuntimeMethod_var), NULL);
		Func_2_t75667D71F159AEB8D73106C0895991743541AD05* L_26 = L_25;
		((U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var))->___U3CU3E9__26_0_1 = L_26;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var))->___U3CU3E9__26_0_1), (void*)L_26);
		G_B2_0 = L_26;
		G_B2_1 = G_B1_1;
	}

IL_00bb:
	{
		RuntimeObject* L_27;
		L_27 = Enumerable_Select_TisIAndroidJavaObjectWrapper_tC1A612D0FB5242E0B7B6FE0D545945956CFF7DF4_TisString_t_m762C1F9DB2640F02FBC59A9060856067CB3119E3(G_B2_1, G_B2_0, Enumerable_Select_TisIAndroidJavaObjectWrapper_tC1A612D0FB5242E0B7B6FE0D545945956CFF7DF4_TisString_t_m762C1F9DB2640F02FBC59A9060856067CB3119E3_RuntimeMethod_var);
		List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* L_28;
		L_28 = Enumerable_ToList_TisString_t_m86360148F90DE6EA1A8363F38B7C2A88FD139131(L_27, Enumerable_ToList_TisString_t_m86360148F90DE6EA1A8363F38B7C2A88FD139131_RuntimeMethod_var);
		V_0 = L_28;
		// receipt = GoogleReceiptEncoder.EncodeReceipt(
		//     originalJson,
		//     signature,
		//     skuDetailsJson
		// );
		String_t* L_29;
		L_29 = GooglePurchase_get_originalJson_m6708011BD0AE03F2280CD86A0F07875EA578D5BA_inline(__this, NULL);
		String_t* L_30;
		L_30 = GooglePurchase_get_signature_m72063440F5794869DB8A4DE3F56A73F4444786AC_inline(__this, NULL);
		List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* L_31 = V_0;
		String_t* L_32;
		L_32 = GoogleReceiptEncoder_EncodeReceipt_m17FC37EB777C0CD19B0A1345C320C17F030911D8(L_29, L_30, L_31, NULL);
		__this->___U3CreceiptU3Ek__BackingField_4 = L_32;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___U3CreceiptU3Ek__BackingField_4), (void*)L_32);
		// }
		return;
	}
}
// System.Boolean UnityEngine.Purchasing.Models.GooglePurchase::IsAcknowledged()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool GooglePurchase_IsAcknowledged_mE2F920ABCC295EA6F298E0AA74B4C3097C58F889 (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidJavaObjectWrapper_Call_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_m5F286AAE3D9A081053469736CC66C2873A0A9900_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral001453AEE96196C60F5094DBB37BD7779972F12D);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return javaPurchase != null && javaPurchase.Call<bool>("isAcknowledged");
		RuntimeObject* L_0;
		L_0 = GooglePurchase_get_javaPurchase_m82A168C3FB80849E2B85BED12EE4DCA6E58CEC18_inline(__this, NULL);
		if (!L_0)
		{
			goto IL_001e;
		}
	}
	{
		RuntimeObject* L_1;
		L_1 = GooglePurchase_get_javaPurchase_m82A168C3FB80849E2B85BED12EE4DCA6E58CEC18_inline(__this, NULL);
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_2;
		L_2 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_1);
		bool L_3;
		L_3 = GenericInterfaceFuncInvoker2< bool, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* >::Invoke(IAndroidJavaObjectWrapper_Call_TisBoolean_t09A6377A54BE2F9E6985A8149F19234FD7DDFE22_m5F286AAE3D9A081053469736CC66C2873A0A9900_RuntimeMethod_var, L_1, _stringLiteral001453AEE96196C60F5094DBB37BD7779972F12D, L_2);
		return L_3;
	}

IL_001e:
	{
		return (bool)0;
	}
}
// System.Boolean UnityEngine.Purchasing.Models.GooglePurchase::IsPurchased()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool GooglePurchase_IsPurchased_m0091EC5B71B28E403588B26FD73EC2C0A19D36D1 (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// return javaPurchase != null && purchaseState == GooglePurchaseStateEnum.Purchased();
		RuntimeObject* L_0;
		L_0 = GooglePurchase_get_javaPurchase_m82A168C3FB80849E2B85BED12EE4DCA6E58CEC18_inline(__this, NULL);
		if (!L_0)
		{
			goto IL_0016;
		}
	}
	{
		int32_t L_1;
		L_1 = GooglePurchase_get_purchaseState_m25B05A607B60519FBA52843CAEF8FD8FEE0752A9_inline(__this, NULL);
		int32_t L_2;
		L_2 = GooglePurchaseStateEnum_Purchased_m3791A59F7885C918735F78345549C35C39E661F0(NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)L_2))? 1 : 0);
	}

IL_0016:
	{
		return (bool)0;
	}
}
// System.Boolean UnityEngine.Purchasing.Models.GooglePurchase::IsPending()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR bool GooglePurchase_IsPending_mB50CFCB4540C15FEEE6853C95CE3155C3D4C9E66 (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// return javaPurchase != null && purchaseState == GooglePurchaseStateEnum.Pending();
		RuntimeObject* L_0;
		L_0 = GooglePurchase_get_javaPurchase_m82A168C3FB80849E2B85BED12EE4DCA6E58CEC18_inline(__this, NULL);
		if (!L_0)
		{
			goto IL_0016;
		}
	}
	{
		int32_t L_1;
		L_1 = GooglePurchase_get_purchaseState_m25B05A607B60519FBA52843CAEF8FD8FEE0752A9_inline(__this, NULL);
		int32_t L_2;
		L_2 = GooglePurchaseStateEnum_Pending_m419C6870D3097EADAF00FF0D6FF5C486BFB13171(NULL);
		return (bool)((((int32_t)L_1) == ((int32_t)L_2))? 1 : 0);
	}

IL_0016:
	{
		return (bool)0;
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
// System.Void UnityEngine.Purchasing.Models.GooglePurchase/<>c::.cctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__cctor_m3B5205D71CD68DEE8540207194DC751BCC9794B5 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var);
		s_Il2CppMethodInitialized = true;
	}
	{
		U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2* L_0 = (U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2*)il2cpp_codegen_object_new(U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		U3CU3Ec__ctor_m696F4E3E542DD5C7ADEFA41805FB149F796B836A(L_0, NULL);
		((U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var))->___U3CU3E9_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&((U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_StaticFields*)il2cpp_codegen_static_fields_for(U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2_il2cpp_TypeInfo_var))->___U3CU3E9_0), (void*)L_0);
		return;
	}
}
// System.Void UnityEngine.Purchasing.Models.GooglePurchase/<>c::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void U3CU3Ec__ctor_m696F4E3E542DD5C7ADEFA41805FB149F796B836A (U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		return;
	}
}
// System.String UnityEngine.Purchasing.Models.GooglePurchase/<>c::<.ctor>b__26_0(UnityEngine.Purchasing.Utils.IAndroidJavaObjectWrapper)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* U3CU3Ec_U3C_ctorU3Eb__26_0_mB666CC9852E094F68F830014EA039871BBF416BA (U3CU3Ec_t5F4A44F3BE5DBDC253279EFFC260CCE4AC510CC2* __this, RuntimeObject* ___0_skuDetail, const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&IAndroidJavaObjectWrapper_Call_TisString_t_m1A02C80883EF91CD3314D0856FE96818794AA538_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral9303FDBBA3EA9F42A781A1107ABF8F1702BF684C);
		s_Il2CppMethodInitialized = true;
	}
	{
		// var skuDetailsJson = skuDetails.Select(skuDetail => skuDetail.Call<string>("getOriginalJson")).ToList();
		RuntimeObject* L_0 = ___0_skuDetail;
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_1;
		L_1 = Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_inline(Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_RuntimeMethod_var);
		NullCheck(L_0);
		String_t* L_2;
		L_2 = GenericInterfaceFuncInvoker2< String_t*, String_t*, ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* >::Invoke(IAndroidJavaObjectWrapper_Call_TisString_t_m1A02C80883EF91CD3314D0856FE96818794AA538_RuntimeMethod_var, L_0, _stringLiteral9303FDBBA3EA9F42A781A1107ABF8F1702BF684C, L_1);
		return L_2;
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
// UnityEngine.AndroidJavaObject UnityEngine.Purchasing.Models.GooglePurchaseStateEnum::GetPurchaseStateJavaObject()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* GooglePurchaseStateEnum_GetPurchaseStateJavaObject_mBEFD71488906CB2105D270DACD285AFFE95C89E1 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralF3DB8521ADB71488B0A3D538F58F98B35E326552);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return new AndroidJavaClass(k_AndroidPurchaseStateClassName);
		AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03* L_0 = (AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03*)il2cpp_codegen_object_new(AndroidJavaClass_tE6296B30CC4BF84434A9B765267F3FD0DD8DDB03_il2cpp_TypeInfo_var);
		NullCheck(L_0);
		AndroidJavaClass__ctor_mB5466169E1151B8CC44C8FED234D79984B431389(L_0, _stringLiteralF3DB8521ADB71488B0A3D538F58F98B35E326552, NULL);
		return L_0;
	}
}
// System.Int32 UnityEngine.Purchasing.Models.GooglePurchaseStateEnum::Purchased()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t GooglePurchaseStateEnum_Purchased_m3791A59F7885C918735F78345549C35C39E661F0 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteral6DAB35E4EA4BBD5AF1473155FA1288D974D1DAD9);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return GetPurchaseStateJavaObject().GetStatic<int>("PURCHASED");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_0;
		L_0 = GooglePurchaseStateEnum_GetPurchaseStateJavaObject_mBEFD71488906CB2105D270DACD285AFFE95C89E1(NULL);
		NullCheck(L_0);
		int32_t L_1;
		L_1 = AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC(L_0, _stringLiteral6DAB35E4EA4BBD5AF1473155FA1288D974D1DAD9, AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC_RuntimeMethod_var);
		return L_1;
	}
}
// System.Int32 UnityEngine.Purchasing.Models.GooglePurchaseStateEnum::Pending()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t GooglePurchaseStateEnum_Pending_m419C6870D3097EADAF00FF0D6FF5C486BFB13171 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC_RuntimeMethod_var);
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralE621E6581BCE23AE171A5EFE8813FCCCF6DC45FF);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return GetPurchaseStateJavaObject().GetStatic<int>("PENDING");
		AndroidJavaObject_t8FFB930F335C1178405B82AC2BF512BB1EEF9EB0* L_0;
		L_0 = GooglePurchaseStateEnum_GetPurchaseStateJavaObject_mBEFD71488906CB2105D270DACD285AFFE95C89E1(NULL);
		NullCheck(L_0);
		int32_t L_1;
		L_1 = AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC(L_0, _stringLiteralE621E6581BCE23AE171A5EFE8813FCCCF6DC45FF, AndroidJavaObject_GetStatic_TisInt32_t680FF22E76F6EFAD4375103CBBFFA0421349384C_m740F3401DEA4A75BADD753EFF71D2328B4147BFC_RuntimeMethod_var);
		return L_1;
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
// System.Int32 UnityEngine.Purchasing.Models.GooglePurchaseStateEnumProvider::Purchased()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t GooglePurchaseStateEnumProvider_Purchased_m367280B3C4A0D25DE27159A38A1F7E8E10835F40 (GooglePurchaseStateEnumProvider_t4F9C48DADF977FD31FFE29D767F092126332683A* __this, const RuntimeMethod* method) 
{
	{
		// return GooglePurchaseStateEnum.Purchased();
		int32_t L_0;
		L_0 = GooglePurchaseStateEnum_Purchased_m3791A59F7885C918735F78345549C35C39E661F0(NULL);
		return L_0;
	}
}
// System.Int32 UnityEngine.Purchasing.Models.GooglePurchaseStateEnumProvider::Pending()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR int32_t GooglePurchaseStateEnumProvider_Pending_mDF35C16DB0772027E6013DFBA15969B13E3C0B75 (GooglePurchaseStateEnumProvider_t4F9C48DADF977FD31FFE29D767F092126332683A* __this, const RuntimeMethod* method) 
{
	{
		// return GooglePurchaseStateEnum.Pending();
		int32_t L_0;
		L_0 = GooglePurchaseStateEnum_Pending_m419C6870D3097EADAF00FF0D6FF5C486BFB13171(NULL);
		return L_0;
	}
}
// System.Void UnityEngine.Purchasing.Models.GooglePurchaseStateEnumProvider::.ctor()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void GooglePurchaseStateEnumProvider__ctor_mBE9E27B95EC11A8AD90B102BF49D0DD6CCA80780 (GooglePurchaseStateEnumProvider_t4F9C48DADF977FD31FFE29D767F092126332683A* __this, const RuntimeMethod* method) 
{
	{
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
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
// System.String UnityEngine.Purchasing.Models.GoogleSkuTypeEnum::InApp()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GoogleSkuTypeEnum_InApp_m3D8DF28E36C52A558A171EBE49300FE42E73C0B9 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralA44250C90C4461C6F602B3B9DC9B873627787D3B);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return "inapp";
		return _stringLiteralA44250C90C4461C6F602B3B9DC9B873627787D3B;
	}
}
// System.String UnityEngine.Purchasing.Models.GoogleSkuTypeEnum::Sub()
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR String_t* GoogleSkuTypeEnum_Sub_m67C8DA9DA489930486A1A308049B9C52C2C071C3 (const RuntimeMethod* method) 
{
	static bool s_Il2CppMethodInitialized;
	if (!s_Il2CppMethodInitialized)
	{
		il2cpp_codegen_initialize_runtime_metadata((uintptr_t*)&_stringLiteralC5EE8C59C90DE1E698A3010542A9B964C720ED30);
		s_Il2CppMethodInitialized = true;
	}
	{
		// return "subs";
		return _stringLiteralC5EE8C59C90DE1E698A3010542A9B964C720ED30;
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
// System.Void UnityEngine.Purchasing.Models.ProductDescriptionQuery::.ctor(System.Collections.ObjectModel.ReadOnlyCollection`1<UnityEngine.Purchasing.ProductDefinition>,System.Action`1<System.Collections.Generic.List`1<UnityEngine.Purchasing.Extension.ProductDescription>>,System.Action`1<UnityEngine.Purchasing.Models.GoogleRetrieveProductsFailureReason>)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR void ProductDescriptionQuery__ctor_m359F071042E9EC689BA607F3D222FB59EB0DE04B (ProductDescriptionQuery_t03B36576574F6E71672313472421EE2FB8C5BFAE* __this, ReadOnlyCollection_1_tA49701F42E3782EB8804C53D26901317BAD43A9E* ___0_products, Action_1_tA72D33CF2F54A3A2B5EA5FC85BF59006A8BCC2BE* ___1_onProductsReceived, Action_1_t827DD5268A2273F9357827BCDAB0FD15F77CD462* ___2_onRetrieveProductsFailed, const RuntimeMethod* method) 
{
	{
		// internal ProductDescriptionQuery(ReadOnlyCollection<ProductDefinition> products, Action<List<ProductDescription>> onProductsReceived, Action<GoogleRetrieveProductsFailureReason> onRetrieveProductsFailed)
		Object__ctor_mE837C6B9FA8C6D5D109F4B2EC885D79919AC0EA2(__this, NULL);
		// this.products = products;
		ReadOnlyCollection_1_tA49701F42E3782EB8804C53D26901317BAD43A9E* L_0 = ___0_products;
		__this->___products_0 = L_0;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___products_0), (void*)L_0);
		// this.onProductsReceived = onProductsReceived;
		Action_1_tA72D33CF2F54A3A2B5EA5FC85BF59006A8BCC2BE* L_1 = ___1_onProductsReceived;
		__this->___onProductsReceived_1 = L_1;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___onProductsReceived_1), (void*)L_1);
		// this.onRetrieveProductsFailed = onRetrieveProductsFailed;
		Action_1_t827DD5268A2273F9357827BCDAB0FD15F77CD462* L_2 = ___2_onRetrieveProductsFailed;
		__this->___onRetrieveProductsFailed_2 = L_2;
		Il2CppCodeGenWriteBarrier((void**)(&__this->___onRetrieveProductsFailed_2), (void*)L_2);
		// }
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
// System.UInt32 <PrivateImplementationDetails>::ComputeStringHash(System.String)
IL2CPP_EXTERN_C IL2CPP_METHOD_ATTR uint32_t U3CPrivateImplementationDetailsU3E_ComputeStringHash_m88B6F9ABC0B2644814DC58FB9602948572F7E971 (String_t* ___0_s, const RuntimeMethod* method) 
{
	uint32_t V_0 = 0;
	int32_t V_1 = 0;
	{
		String_t* L_0 = ___0_s;
		if (!L_0)
		{
			goto IL_002a;
		}
	}
	{
		V_0 = ((int32_t)-2128831035);
		V_1 = 0;
		goto IL_0021;
	}

IL_000d:
	{
		String_t* L_1 = ___0_s;
		int32_t L_2 = V_1;
		NullCheck(L_1);
		Il2CppChar L_3;
		L_3 = String_get_Chars_mC49DF0CD2D3BE7BE97B3AD9C995BE3094F8E36D3(L_1, L_2, NULL);
		uint32_t L_4 = V_0;
		V_0 = ((int32_t)il2cpp_codegen_multiply(((int32_t)((int32_t)L_3^(int32_t)L_4)), ((int32_t)16777619)));
		int32_t L_5 = V_1;
		V_1 = ((int32_t)il2cpp_codegen_add(L_5, 1));
	}

IL_0021:
	{
		int32_t L_6 = V_1;
		String_t* L_7 = ___0_s;
		NullCheck(L_7);
		int32_t L_8;
		L_8 = String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline(L_7, NULL);
		if ((((int32_t)L_6) < ((int32_t)L_8)))
		{
			goto IL_000d;
		}
	}

IL_002a:
	{
		uint32_t L_9 = V_0;
		return L_9;
	}
}
#ifdef __clang__
#pragma clang diagnostic pop
#endif
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* GooglePurchase_get_skus_mFB5A449AA1EE9433CFE668CDE90A55B7FDEB81A4_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public List<string> skus { get; }
		List_1_tF470A3BE5C1B5B68E1325EF3F109D172E60BD7CD* L_0 = __this->___U3CskusU3Ek__BackingField_2;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* GooglePurchase_get_originalJson_m6708011BD0AE03F2280CD86A0F07875EA578D5BA_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public string originalJson { get; }
		String_t* L_0 = __this->___U3CoriginalJsonU3Ek__BackingField_6;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR String_t* GooglePurchase_get_signature_m72063440F5794869DB8A4DE3F56A73F4444786AC_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public string signature { get; }
		String_t* L_0 = __this->___U3CsignatureU3Ek__BackingField_5;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR RuntimeObject* GooglePurchase_get_javaPurchase_m82A168C3FB80849E2B85BED12EE4DCA6E58CEC18_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public IAndroidJavaObjectWrapper javaPurchase { get; }
		RuntimeObject* L_0 = __this->___U3CjavaPurchaseU3Ek__BackingField_0;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t GooglePurchase_get_purchaseState_m25B05A607B60519FBA52843CAEF8FD8FEE0752A9_inline (GooglePurchase_tFABB74E360ED620F60451B0E688B98BA378C0EDF* __this, const RuntimeMethod* method) 
{
	{
		// public int purchaseState { get; }
		int32_t L_0 = __this->___U3CpurchaseStateU3Ek__BackingField_1;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR int32_t String_get_Length_m42625D67623FA5CC7A44D47425CE86FB946542D2_inline (String_t* __this, const RuntimeMethod* method) 
{
	{
		int32_t L_0 = __this->____stringLength_4;
		return L_0;
	}
}
IL2CPP_MANAGED_FORCE_INLINE IL2CPP_METHOD_ATTR ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* Array_Empty_TisRuntimeObject_mFB8A63D602BB6974D31E20300D9EB89C6FE7C278_gshared_inline (const RuntimeMethod* method) 
{
	{
		il2cpp_codegen_runtime_class_init_inline(il2cpp_rgctx_data(method->rgctx_data, 0));
		ObjectU5BU5D_t8061030B0A12A55D5AD8652A20C922FE99450918* L_0 = ((EmptyArray_1_tDF0DD7256B115243AA6BD5558417387A734240EE_StaticFields*)il2cpp_codegen_static_fields_for(il2cpp_rgctx_data(method->rgctx_data, 0)))->___Value_0;
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
