using UnityEngine;
using System.Collections;
using System;



public class JNetworkMgr : MonoBehaviour
{
	public Action<LoginResult> NtfGetLoginResult;

	public static JNetworkMgr ins_;

	public JNetworkMgr()
	{
		JNetworkMgr.ins_ = this;
	}

}
