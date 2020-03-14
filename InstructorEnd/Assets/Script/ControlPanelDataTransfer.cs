﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ControlPanelDataTransfer : MonoBehaviourPun
{
    public PhotonView pv;
    public ControlPanelManagement controlPanelManagement;

    private bool _lightState;
    private string Vitalname;
    private string Vitalnumber;
    private string objName;
    private bool objState;
    private GameObject vitalmonitor;
    private GameObject changeables;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();

        controlPanelManagement = GameObject.Find("TrainingManager").GetComponent<ControlPanelManagement>();
        vitalmonitor = GameObject.Find("VitalPanel/Panel/RawImage");
        changeables = GameObject.Find("TraumaBay/Changeables");
    }

    // Update is called once per frame
    void Update()
    {
        if (_lightState != controlPanelManagement.lightStates)
        {
            pv.RPC("RPC_LightSwitch", RpcTarget.All, controlPanelManagement.lightStates);
            _lightState = controlPanelManagement.lightStates;
        }
        if (controlPanelManagement.vis_changed)
        {
            objName = controlPanelManagement.objname;
            objState = controlPanelManagement.objvis;
            pv.RPC("RPC_ToggleVis", RpcTarget.AllBuffered, objName, objState);
            controlPanelManagement.vis_changed = false;
        }
        if (controlPanelManagement.update_vitals)
        {
            Vitalname = controlPanelManagement.vitalName;
            Vitalnumber = controlPanelManagement.vitalNumber;
            pv.RPC("RPC_VitalChange", RpcTarget.AllBuffered, Vitalname, Vitalnumber);
            controlPanelManagement.update_vitals = false;
        }
    }

    [PunRPC]
    public void RPC_LightSwitch(bool lightStates)
    {
        controlPanelManagement.lights.SetActive(lightStates);
        Debug.Log(lightStates);
    }

    [PunRPC]
    public void RPC_VitalChange(string obj, string numbers)
    {
        vitalmonitor.transform.Find(obj +"/Number").GetComponent<Text>().text = numbers;
    }

    [PunRPC]
    public void RPC_ToggleVis(string obj, bool val)
    {
        GameObject o = changeables.transform.Find(obj).gameObject;
        o.SetActive(val);
    }
}
