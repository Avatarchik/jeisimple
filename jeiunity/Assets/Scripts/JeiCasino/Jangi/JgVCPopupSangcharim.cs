using UnityEngine;
using System.Collections;
using jangi;
using UnityEngine.UI;

public class JgVCPopupSangcharim : JViewController
{
	public JgNation nation;
	public GameObject hanSetting;
	public Image loadingSpinner;

	public Image[] sangcharimButtons;

	jangi.Sangcharim sangcharim = Sangcharim.kSMSM;


	const string Key_Sangcharim = "Sangcharim";

	void OnEnable()
	{
		JgRefs.ins_.msg.onNtfSancharimHan += OnNtfSangcharimHan;

		UpdateSangcharimUI();
	}
	void OnDisable()
	{
		JgRefs.ins_.msg.onNtfSancharimHan -= OnNtfSangcharimHan;
	}

	void UpdateSangcharimUI()
	{
		if (PlayerPrefs.HasKey(Key_Sangcharim))
			this.sangcharim = (Sangcharim)PlayerPrefs.GetInt(Key_Sangcharim);
		
		foreach (Image img in sangcharimButtons)
			img.color = Color.gray;
		
		sangcharimButtons[(int)sangcharim - 1].color = Color.white;
	}

	public void ShowPopup() //AnsReqMatch ans)
	{
		//ansReqMatch = ans;
		//this.isCho = ans.IsCho;
		if (nation == JgNation.kCho)
			SetWaitingHanType();

		JVCMgr.ins_.PushView(this);
	}

	public void OnClick_MaSangCombination(int sangcharim)
	{
		this.sangcharim = (jangi.Sangcharim)sangcharim;

		Debug.Log("selected: " + this.sangcharim + "\n");

		foreach (Image img in sangcharimButtons)
			img.color = Color.gray;

		sangcharimButtons[sangcharim - 1].color = Color.white;
	}
	public void OnClick_Ok()
	{
		ReqSangcharim req = new ReqSangcharim();
		req.Sangcharim = sangcharim;
		JgRefs.ins_.msg.SendMessageToServer(JgC2S_Msgs.kSangcharim, req);
	
		Debug.Log(string.Format("AppPlayer has chosen {0} \n", sangcharim));

		PlayerPrefs.SetInt(Key_Sangcharim, (int)this.sangcharim);

		JVCMgr.ins_.PopView();
		JgRefs.ins_.vcInGame.Show(true);
		//JVCMgr.ins_.PushView(JgRefs.ins_.vcInGame);
	}

	void OnNtfSangcharimHan(NtfSangcharimHan han)
	{
		hanSetting.SetActive(true);
		loadingSpinner.gameObject.SetActive(false);
	}

	void SetWaitingHanType()
	{
		hanSetting.SetActive(false);
		loadingSpinner.gameObject.SetActive(true);
	}
}
