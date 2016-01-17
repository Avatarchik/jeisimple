
namespace * jangi

enum JgC2S_MessageType
{
	kUnknown,
	kLogin,
	kChannelInfo,
	kReqMatch,
	kReqCancelMatch,
	kSangcharim,
	kMovePawn,
	kPassTurn
}
enum JgS2C_MessageType
{
	kUnknown,
	kNtfTestString,

	kLogin,
	kChannelInfo,
	kNtfMatch,
	kCancelMatch,
	kNtfSangcharimHan,
	kNtfSangcharim,
	kNtfChangeTurn,
	kNtfMovePawn,
}

struct UserInfo
{
	1: string nickName
	2: i32 level
	3: i32 victoryCount
	4: i32 defeatCount
}

enum Sangcharim
{
	kUnknown,
	kSMSM,
	kMSMS,
	kMSSM,
	kSMMS	
}

/////////////////////////////////////////////////////////////////////////
struct NtfTestString
{
	1: string message	
}

struct ReqLogin
{
	1: string userName
	2: string loginPlatform
	3: string deviceDesc
}

struct AnsChannelInfo
{
	1: i32 count
}

struct AnsLogin
{
	1: bool   	loginOk
	2: string 	nickName
	3: i32 		level
	4: i32 		victoryCount
	5: i32 		defeatCount
	6: optional string comment
	7: optional AnsChannelInfo channelInfo
}

struct ReqChannelInfo
{
	
}

//----------------------------------------
// InGame View
struct ReqMatch
{
}
struct NtfMatch
{
	1: i32 localId
//	2: UserInfo opponentInfo
}

struct ReqCancelRequestMatch
{
}
struct AnsCancelRequestMatch
{
}

struct ReqSangcharim
{
	1: Sangcharim sangcharim
}
struct NtfSangcharimHan
{
	1: Sangcharim han
}
struct NtfSangcharim
{
	1: Sangcharim cho
	2: Sangcharim han
}


struct ReqMovePawn
{
	1: i32 location
	2: i32 target
}
struct NtfChangeTurn
{
	1: i32 localId
}
struct NtfMovePawn
{
	1: i32 localId
	2: i32 location
	3: i32 target
	4: i32 dummy
}
