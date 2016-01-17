
namespace * jcasino


enum SevenCardStudBetType
{
	kUnknown = 0
	kDie
	kPping
	kTadang
	kCall
	kCheck
	kQuater
	kHalf
}

enum SevenCardStudHand
{
	kUnknown = 0
	kTop
	kOnePair
	kTwoPair
	kThreeCard
	Straight
	Mountain
	Flush
	Fullhouse
	FourCard
	RoyalStraightFlush
}

struct HandInfo
{
	1: i32 			playerLocalId
	2: list<i32> 	hand
}

struct HandInfoOpted
{
	1: binary playerHand
}
