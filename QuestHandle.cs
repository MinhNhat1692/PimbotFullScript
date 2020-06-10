
public bool TalkWithNPC(string NpcName,ushort offset = 0){  
    NpcInMapEntity NPCInfomation = FindNPCNoInCurrentMapByName(NpcName,offset);
    ts.Debug("Moving Close To NPC");
    ts.Move(ushort.Parse((NPCInfomation.PosX - 1).ToString()),ushort.Parse((NPCInfomation.PosY - 1).ToString()));
    ts.Delay(TimeDelayAction);
    ts.Debug("Talk to NPC " + NPCInfomation.NpcNo.ToString() + " - Name: " + NPCInfomation.NpcName);
    ts.ClickNpc(NPCInfomation.NpcNo);
    ts.Delay(TimeDelayAction);
    return true;
}

public void ActiveEvent(ushort EventId){
    ts.Debug("Active Event " + EventId.ToString());
    ts.ActiveEvent(EventId);
    ts.Delay(TimeDelayAction);
}

public void HandlingPmapMove(ushort WarpID,ushort DestId,string DestName,bool isTeleport){
            if (DestId != 0){
                if (isTeleport){
                    ts.Debug("Teleporting to map " + DestName);
                    ts.Teleport(DestId,byte.Parse(WarpID.ToString()));
                    ts.Delay(TimeDelayAction);    
                }else{
                    ts.Debug("Warping port " + WarpID.ToString() + " to map " + DestName);
                    ts.Warp(WarpID);
                    ts.Delay(TimeDelayAction);
                }
            }
        }
        