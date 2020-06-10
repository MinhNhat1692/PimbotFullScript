        bool isWarping = false;
		string buyHPComand = "buyhp";
		string buySPComand = "buysp";
		string SellNDComand = "sellnd";
        string FindNpcCommand = "findNPC";
        string pmapCommand = "pmap";
        string questKeChiemLinhLang = "quest_KeChiemLinhLang";
        string questHoanThanhDaiChien = "quest_HoanThanhDaiChien";
        int TimeDelayAction = 3000;
        bool isBattle=false;
		bool isDaChuyenKenhVang=false;
		ushort nowX, nowY;
		bool isNowXY=false;
		ushort KenhChiDinh = 1;
		bool isVaoDungKenh = false;
        bool isBattleWaitting = false;
        int questStep = 0;
        bool inQuest = false;
        /*private bool TalkWithNPC(string NpcName,ushort offset = 0){
            NpcInMapEntity NPCInfomation = FindNPCNoInCurrentMapByName(NpcName,offset);
            ts.Debug("Moving Close To NPC");
            ts.Move(ushort.Parse((NPCInfomation.PosX - 1).ToString()),ushort.Parse((NPCInfomation.PosY - 1).ToString()));
            ts.Delay(TimeDelayAction);
            ts.Debug("Talk to NPC " + NPCInfomation.NpcNo.ToString() + " - Name: " + NPCInfomation.NpcName);
            ts.ClickNpc(NPCInfomation.NpcNo);
            ts.Delay(TimeDelayAction);
            return true;
        }*/
        private void WaitBattleEnd(){
            isBattleWaitting = true;
            while (isBattleWaitting){
                ts.Debug("Waitting Battle");
                ts.Delay(5*TimeDelayAction);
            }
        }
        /*
        private void ActiveEvent(ushort EventId){
            ts.Debug("Active Event " + EventId.ToString());
            ts.ActiveEvent(EventId);
            ts.Delay(TimeDelayAction);
        }*/

        /*private void HandlingPmapMove(ushort WarpID,ushort DestId,string DestName,bool isTeleport){
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
        }*/

        private bool pmapFromMapToMap(ushort end, bool isTeleport =false){
            ts.Debug("Pmaping to map " + end.ToString());
            var Route = ts.pmap.Get(ts.Character.MapId, end, isTeleport);
            for (int i=0;i<Route.Count();i++){
                while(isBattle){
                    ts.Delay(5*TimeDelayAction);
                }
                HandlingPmapMove(Route[i].WarpId,Route[i].DestId,Route[i].DestName,Route[i].isTeleport);
            }
            return true;
        }

        private void AutoResponseDialogType(byte DialogType, int DialogId){
            System.Threading.Tasks.Task.Run(() =>{ //Gắn vào Task (tương tự Multi Thread, để khong ảnh hưởng tới packet BOT
                ts.Delay(TimeDelayAction); //Tạm ngưng 2s trước khi xử lý, nếu không sẽ bị dis
                if (DialogType == 0 && DialogId == 0){
                    ts.Debug("Handling Dialog Type 0");   
                    ts.ChoiceDialog(0);
                }
            });
        }

        private void PrintListNpcCurrentMap(){
            System.Threading.Tasks.Task.Run(() =>{ //Gắn vào Task (tương tự Multi Thread, để khong ảnh hưởng tới packet BOT
                ts.Delay(TimeDelayAction); //Tạm ngưng 2s trước khi xử lý, nếu không sẽ bị dis
                foreach (var NPC in ts.listNpcInCurrentMap){
                    ts.Debug("NPC No" + NPC.NpcNo.ToString() + " - Name: " + NPC.NpcName.ToString() + " - X:" + NPC.PosX.ToString() + " - Y:" + NPC.PosY.ToString() + " - Event:" + NPC.Event.ToString());
                }
            });
        }

        private NpcInMapEntity FindNPCNoInCurrentMapByName(string NpcName,ushort offset = 0){
            ushort i = 0;
            foreach (var NPC in ts.listNpcInCurrentMap){
                if (NPC.NpcName.ToString().Contains(NpcName)){
                    if (i == offset){
                        ts.Debug("NPC No" + NPC.NpcNo.ToString() + " - Name: " + NPC.NpcName.ToString() + " - X:" + NPC.PosX.ToString() + " - Y:" + NPC.PosY.ToString() + " - Event:" + NPC.Event.ToString());
                        //ts.Debug(NPC);
                        return NPC;
                    }else{
                        i++;
                    } 
                }
            }
            return null;
        }

		private void AutoBuyHp(int number = 2){  
            System.Threading.Tasks.Task.Run(() =>{ //Gắn vào Task (tương tự Multi Thread, để khong ảnh hưởng tới packet BOT
                ts.Delay(2000); //Tạm ngưng 2s trước khi xử lý, nếu không sẽ bị dis
				int timedelay = 1000;
                switch(ts.Character.MapId){
                    case 12001: //Nếu ở Trác Quận
                        ts.Warp(1); // Ra map 63000
                        ts.Delay(timedelay);
                        ts.Warp(5);
                        ts.Delay(timedelay);
                        ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(1); // Chọn option là MUA
                        ts.Delay(timedelay);
						for(int i = 0; i < number; i++) // chọn số slot cần mua HP, hiện tại là 2
                        {
                            ts.Buy(0,999); //Mua vị trí HP đầu tiên trong bản MUA, với số lượng là 999.
                            ts.Delay(timedelay);
                        }
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(12001,0);
                        break;
                    case 63000:
                        ts.Warp(5);
                        ts.Delay(timedelay);
                        ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(1); // Chọn option là MUA
                        ts.Delay(timedelay);
						for(int i = 0; i < number; i++) // chọn số slot cần mua HP, hiện tại là 2
                        {
                            ts.Buy(0,999); //Mua vị trí HP đầu tiên trong bản MUA, với số lượng là 999.
                            ts.Delay(timedelay);
                        }
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(63000,0);
                        break;
                    case 12992: //Trại hương dũng
						ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(1); // Chọn option là MUA
                        ts.Delay(timedelay);
						for(int i = 0; i < number; i++) // chọn số slot cần mua HP, hiện tại là 2
                        {
                            ts.Buy(0,999); //Mua vị trí HP đầu tiên trong bản MUA, với số lượng là 999.
                            ts.Delay(timedelay);
                        }
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        break;
                    default:
                        ts.Teleport(12001,0); // ở bất kì map nào, Teleport về Trác Quận
                        ts.Delay(timedelay);
                        ts.Warp(1); // Ra map 63000
                        ts.Delay(timedelay);
                        ts.Warp(5);
                        ts.Delay(timedelay);
                        ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(1); // Chọn option là MUA
                        ts.Delay(timedelay);
						for(int i = 0; i < number; i++) // chọn số slot cần mua HP, hiện tại là 2
                        {
                            ts.Buy(0,999); //Mua vị trí HP đầu tiên trong bản MUA, với số lượng là 999.
                            ts.Delay(timedelay);
                        }
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(12001,0);
                        break;
                }
            });          
        }

        private void LucKhauThatKe(){  
            System.Threading.Tasks.Task.Run(() =>{ //Gắn vào Task (tương tự Multi Thread, để khong ảnh hưởng tới packet BOT
                ts.Delay(2000); //Tạm ngưng 2s trước khi xử lý, nếu không sẽ bị dis
				int timedelay = 1000;
                ts.Teleport(15001,0);
                ts.Delay(timedelay);
                ts.Warp(2);
                ts.Delay(timedelay);
                ts.Warp(18);
                ts.Delay(timedelay);
                ts.Warp(1);
                ts.Delay(timedelay);
                ts.Warp(6);
                ts.Delay(timedelay);
                ts.Warp(1);
                ts.Delay(timedelay);
                ts.Move(570,530);
                ts.Delay(timedelay);
                ts.ClickNpc(9);
                ts.Delay(timedelay);
                ts.ChoiceDialog(0);
                ts.Delay(timedelay);
                ts.Teleport(18001,0);
                ts.Delay(timedelay);
                ts.Warp(2);
                ts.Delay(timedelay);
                ts.Warp(19);
                ts.Delay(timedelay);
                ts.Warp(1);
                ts.Delay(timedelay);
                ts.Warp(1);
                ts.Delay(timedelay);
                ts.Move(450,410);
                ts.Delay(timedelay);
                ts.ClickNpc(1);
                ts.Delay(timedelay);
            });          
        }

        private void KeChiemLinhLang(int step=0){  
            System.Threading.Tasks.Task.Run(() =>{ //Gắn vào Task (tương tự Multi Thread, để khong ảnh hưởng tới packet BOT
                ts.Delay(2000); //Tạm ngưng 2s trước khi xử lý, nếu không sẽ bị dis
				questStep = step;
                while(inQuest){
                    switch (questStep){
                        case 0:
                            pmapFromMapToMap(21872,true);
                            ts.Delay(TimeDelayAction);
                            questStep = 1;
                            break;
                        case 1:
                            TalkWithNPC("Tôn Quyền");
                            ts.Delay(3*TimeDelayAction);
                            questStep = 2;
                            break;
                        case 2:
                            pmapFromMapToMap(21452,true);
                            ts.Delay(TimeDelayAction);
                            questStep = 3;
                            break;
                        case 3:
                            TalkWithNPC("Lỗ Túc");
                            ts.Delay(TimeDelayAction);
                            questStep = 4;
                            break;
                        case 4:
                            ActiveEvent(5);
                            ts.Delay(3*TimeDelayAction);
                            questStep = 5;
                            break;
                        case 5:
                            pmapFromMapToMap(23803,true);
                            ts.Delay(TimeDelayAction);
                            questStep = 6;
                            break;
                        case 6:
                            TalkWithNPC("Lữ Mông");
                            ts.Delay(TimeDelayAction);
                            questStep = 7;
                            break;
                        case 7:
                            ts.Move(1999,530);
                            ts.Delay(TimeDelayAction);
                            ActiveEvent(4);
                            ts.Delay(TimeDelayAction);
                            questStep = 8;
                            break;
                        case 8:
                            pmapFromMapToMap(23801,false);
                            ts.Delay(TimeDelayAction);
                            questStep = 9;
                            break;
                        case 9:
                            ts.Move(255,519);
                            ts.Delay(TimeDelayAction);
                            ActiveEvent(3);
                            ts.Delay(TimeDelayAction);
                            questStep = 10;
                            break;
                        case 10:
                            pmapFromMapToMap(23003,true);
                            ts.Delay(TimeDelayAction);
                            questStep = 11;
                            break;
                        case 11:
                            TalkWithNPC("Chi");
                            ts.Delay(TimeDelayAction);
                            questStep = 12;
                            break;
                        case 12:
                            TalkWithNPC("Chi");
                            ts.Delay(2*TimeDelayAction);
                            ts.ChoiceDialog(0);
                            ts.Delay(2*TimeDelayAction);
                            ts.ChoiceDialog(0);
                            ts.Delay(2*TimeDelayAction);
                            ts.ChoiceDialog(0);
                            questStep = 13;
                            break;
                        case 13:
                            pmapFromMapToMap(23011,true);
                            ts.Delay(TimeDelayAction);
                            questStep = 14;
                            break;
                        case 14:
                            ts.Move(516,755);
                            ts.Delay(TimeDelayAction);
                            ActiveEvent(5);
                            ts.Delay(TimeDelayAction);
                            //ts.ChoiceDialog(0);
                            questStep = 15;
                            inQuest = false;
                            break;
                    }
                }
            });          
        }

        private void HoanThanhDaiChien(int step=0){  
            System.Threading.Tasks.Task.Run(() =>{ //Gắn vào Task (tương tự Multi Thread, để khong ảnh hưởng tới packet BOT
                ts.Delay(2000); //Tạm ngưng 2s trước khi xử lý, nếu không sẽ bị dis
				questStep = step;
                while(inQuest){
                    switch (questStep){
                        case 0:
                            pmapFromMapToMap(21452,false);
                            ts.Delay(TimeDelayAction);
                            questStep = 1;
                            break;
                        case 1:
                            TalkWithNPC("Tôn Quyền",1);
                            ts.Delay(TimeDelayAction);
                            ts.ClickNpc(11);
                            ts.Delay(TimeDelayAction);
                            ts.ChoiceDialog(0);
                            ts.Delay(3*TimeDelayAction);
                            questStep = 2;
                            break;
                        case 2:
                            pmapFromMapToMap(18511,false);
                            ts.Delay(TimeDelayAction);
                            questStep = 3;
                            break;
                        case 3:
                            TalkWithNPC("Lăng Thống");
                            ts.Delay(TimeDelayAction);
                            questStep = 4;
                            break;
                        case 4:
                            ts.ChoiceDialog(0);
                            ts.Delay(3*TimeDelayAction);
                            WaitBattleEnd();
                            questStep = 5;
                            break;
                        case 5:
                            pmapFromMapToMap(15492,false);
                            ts.Delay(TimeDelayAction);
                            questStep = 6;
                            break;
                        case 6:
                            TalkWithNPC("Tôn Quyền");
                            ts.Delay(TimeDelayAction);
                            questStep = 7;
                            break;
                        case 7:
                            ts.ChoiceDialog(0);
                            ts.Delay(TimeDelayAction);
                            questStep = 8;
                            break;
                        case 8:
                            ts.Move(1103,2252);
                            ts.Delay(TimeDelayAction);
                            ActiveEvent(3);
                            ts.Delay(TimeDelayAction);
                            ts.ChoiceDialog(0);
                            WaitBattleEnd();
                            questStep = 9;
                            break;
                        case 9:
                            ts.Move(512,1886);
                            ts.Delay(TimeDelayAction);
                            ActiveEvent(4);
                            ts.Delay(TimeDelayAction);
                            ts.ChoiceDialog(0);
                            WaitBattleEnd();
                            questStep = 10;
                            break;
                        case 10:
                            ts.Move(2067,857);
                            ts.Delay(TimeDelayAction);
                            ActiveEvent(5);
                            ts.Delay(TimeDelayAction);
                            ts.ChoiceDialog(0);
                            WaitBattleEnd();
                            questStep = 11;
                            break;
                        case 11:
                            ts.Move(774,526);
                            ts.Delay(TimeDelayAction);
                            ActiveEvent(6);
                            ts.Delay(TimeDelayAction);
                            ts.ChoiceDialog(0);
                            WaitBattleEnd();
                            questStep = 12;
                            break;
                        case 12:
                            pmapFromMapToMap(15494,false);
                            ts.Delay(TimeDelayAction);
                            questStep = 13;
                            break;
                        case 13:
                            TalkWithNPC("Tôn Quyền");
                            ts.Delay(2*TimeDelayAction);
                            WaitBattleEnd();
                            questStep = 14;
                            inQuest = false;
                            break;
                    }
                }
            });          
        }
        
	private void AutoBuySp(int number = 2){  
            System.Threading.Tasks.Task.Run(() =>{ //Gắn vào Task (tương tự Multi Thread, để khong ảnh hưởng tới packet BOT
                ts.Delay(2000); //Tạm ngưng 2s trước khi xử lý, nếu không sẽ bị dis
				int timedelay = 1000;
                switch(ts.Character.MapId){
                    case 12001: //Nếu ở Trác Quận
                        ts.Warp(1); // Ra map 63000
                        ts.Delay(timedelay*3);
                        ts.Warp(5);
                        ts.Delay(timedelay*3);
                        ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(1); // Chọn option là MUA
                        ts.Delay(timedelay);
						for(int i = 0; i < number; i++) // chọn số slot cần mua HP, hiện tại là 2
                        {
                            ts.Buy(2,999); //Mua vị trí HP đầu tiên trong bản MUA, với số lượng là 999.
                            ts.Delay(timedelay);
                        }
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(12001,0);
                        break;
                    case 63000:
                        ts.Warp(5);
                        ts.Delay(timedelay*3);
                        ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(1); // Chọn option là MUA
                        ts.Delay(timedelay);
						for(int i = 0; i < number; i++) // chọn số slot cần mua HP, hiện tại là 2
                        {
                            ts.Buy(2,999); //Mua vị trí HP đầu tiên trong bản MUA, với số lượng là 999.
                            ts.Delay(timedelay);
                        }
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(63000,0);
                        break;
                    case 12992: //Trại hương dũng
						ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(1); // Chọn option là MUA
                        ts.Delay(timedelay);
						for(int i = 0; i < number; i++) // chọn số slot cần mua HP, hiện tại là 2
                        {
                            ts.Buy(2,999); //Mua vị trí HP đầu tiên trong bản MUA, với số lượng là 999.
                            ts.Delay(timedelay);
                        }
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        break;
                    default:
                        ts.Teleport(12001,0); // ở bất kì map nào, Teleport về Trác Quận
                        ts.Delay(timedelay*3);
                        ts.Warp(1); // Ra map 63000
                        ts.Delay(timedelay*3);
                        ts.Warp(5);
                        ts.Delay(timedelay);
                        ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(1); // Chọn option là MUA
                        ts.Delay(timedelay);
						for(int i = 0; i < number; i++) // chọn số slot cần mua HP, hiện tại là 2
                        {
                            ts.Buy(0,999); //Mua vị trí HP đầu tiên trong bản MUA, với số lượng là 999.
                            ts.Delay(timedelay);
                        }
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(12001,0);
                        break;
                }
            });          
        }

        private void AutoSellND(){  
            System.Threading.Tasks.Task.Run(() =>{ //Gắn vào Task (tương tự Multi Thread, để khong ảnh hưởng tới packet BOT
                ts.Delay(2000); //Tạm ngưng 2s trước khi xử lý, nếu không sẽ bị dis
				int timedelay = 3000;
				List<ushort> SellList = new List<ushort>();
				SellList.Add(32043); //add thêm nồi đất vào list bán
						var listSell = ts.listItemInBag.FindAll(i => SellList.Contains(i.ItemId) && i.Quantity > 0).ToList();
                switch(ts.Character.MapId){
                    case 12001: //Nếu ở Trác Quận
                        ts.Warp(1); // Ra map 63000
                        ts.Delay(timedelay*3);
                        ts.Warp(5);
                        ts.Delay(timedelay*3);
                        ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(2); // Chọn option là BÁN
                        ts.Delay(timedelay);
						foreach(var item in listSell){
							ts.Sell(item.Index,item.Quantity);
						}
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(12001,0);
                        break;
                    case 63000:
                        ts.Warp(5);
                        ts.Delay(timedelay*3);
                        ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(2); // Chọn option là BÁN
                        ts.Delay(timedelay);
						foreach(var item in listSell){
							ts.Sell(item.Index,item.Quantity);
						}
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(12001,0);
                        break;
                    case 12992: //Trại hương dũng
						ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(2); // Chọn option là BÁN
                        ts.Delay(timedelay);
						foreach(var item in listSell){
							ts.Sell(item.Index,item.Quantity);
						}
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(12001,0);
                        break;
                    default:
                        ts.Teleport(12001,0); // ở bất kì map nào, Teleport về Trác Quận
                        ts.Delay(timedelay*3);
                        ts.Warp(1); // Ra map 63000
                        ts.Delay(timedelay*3);
                        ts.Warp(5);
                        ts.Delay(timedelay);
                        ts.Move(1070,410); // Di chuyển lại gần Thương Nhân
                        ts.Delay(timedelay);
						ts.ClickNpc(12); // Click vào Thương Nhân
                        ts.Delay(timedelay);
                        ts.ChoiceDialog(2); // Chọn option là BÁN
                        ts.Delay(timedelay);
						foreach(var item in listSell){
							ts.Sell(item.Index,item.Quantity);
						}
						ts.ChoiceDialog(0); //Choice 0 đồng nghĩa là tắt bảng MUA BÁN đi.
                        ts.Delay(timedelay);
                        ts.Teleport(12001,0);
                        break;
                }
            });          
        }

		public override void Start()
        {
    // Create a new instance of the form.
    System.Windows.Forms.Form form1 = new System.Windows.Forms.Form();
    // Create two buttons to use as the accept and cancel buttons.
    System.Windows.Forms.Button button1 = new System.Windows.Forms.Button ();
    System.Windows.Forms.Button button2 = new System.Windows.Forms.Button ();
    
    // Set the text of button1 to "OK".
    button1.Text = "OK";
    // Set the position of the button on the form.
    button1.Location = new System.Drawing.Point (10, 10);
    // Set the text of button2 to "Cancel".
    button2.Text = "Cancel";
    // Set the position of the button based on the location of button1.
    button2.Location
        = new System.Drawing.Point (button1.Left, button1.Height + button1.Top + 10);
    // Set the caption bar text of the form.   
    form1.Text = "My Dialog Box";
    // Display a help button on the form.
    form1.HelpButton = true;

    // Define the border style of the form to a dialog box.
    //form1.FormBorderStyle = FormBorderStyle.FixedDialog;
    // Set the MaximizeBox to false to remove the maximize box.
    form1.MaximizeBox = false;
    // Set the MinimizeBox to false to remove the minimize box.
    form1.MinimizeBox = false;
    // Set the accept button of the form to button1.
    form1.AcceptButton = button1;
    // Set the cancel button of the form to button2.
    form1.CancelButton = button2;
    // Set the start position of the form to the center of the screen.
    //form1.StartPosition = FormStartPosition.CenterScreen;
    
    // Add button1 to the form.
    form1.Controls.Add(button1);
    // Add button2 to the form.
    form1.Controls.Add(button2);
    
    // Display the form as a modal dialog box.
    form1.ShowDialog();
}

        
        public override void Stop()
        {

        }
 
        public override void InitBot()
        {
			sw = new System.Diagnostics.Stopwatch();
			//nowX=ts.Character.PosX;
			//nowY=ts.Character.PosY;
			
			
        }
        public override void LoginDone()
        {
        	ts.Debug("Testing TS Debug ");
        }
        public override void OnChat(MessageType type, long uid, string name, string msg)
        {
		char[] spearator = {','};
			switch (type)
            {
                case MessageType.Team:
                   	{
                   		if (ts.Character.Name.Equals(name)){
                   			if (msg.Split(spearator)[0].Equals(buyHPComand)){
                   				ts.Debug("Start HP Schedule");
                   				AutoBuyHp(int.Parse(msg.Split(spearator)[1]));
                   			}else if (msg.Equals(SellNDComand)){
                   				ts.Debug("Start Sell ND Schedule");
                   				AutoSellND();
                   			}else if (msg.Split(spearator)[0].Equals(buySPComand)){
                   				ts.Debug("Start SP Schedule");
                   				AutoBuySp(int.Parse(msg.Split(spearator)[1]));
                   			}else if (msg.Split(spearator)[0].Equals(questKeChiemLinhLang)){
                                ts.Debug("Start quest Ke chiem linh lang");
                                questStep = 0;
                                inQuest = true;
                                KeChiemLinhLang(int.Parse(msg.Split(spearator)[1]));
                            }else if (msg.Split(spearator)[0].Equals(questHoanThanhDaiChien)){
                                ts.Debug("Start quest Hoan Thanh Dai Chien");
                                questStep = 0;
                                inQuest = true;
                                HoanThanhDaiChien(int.Parse(msg.Split(spearator)[1]));
                            }else if (msg.Split(spearator)[0].Equals(FindNpcCommand)){
                                ts.Debug("Get NPC Info");
                                FindNPCNoInCurrentMapByName(msg.Split(spearator)[1]);
                            }else if (msg.Split(spearator)[0].Equals(pmapCommand)){
                                pmapFromMapToMap(ushort.Parse(msg.Split(spearator)[1]),true);
                            }
                   		}
                    }
                    break;
                default: break;
            }
        }
        public override void PlayerAppear(long playerid, string PlayerName, ushort MapId, ushort PosX, ushort PosY, bool isOnline)
        {
        
        }
        public override void PlayerDisappear(long playerid, string PlayerName)
        {
			
        }
        public override void WarpFinish(long playerid, string PlayerName, ushort MapId, ushort PosX, ushort PosY)
        {
        }
        public override void PlayerMove(long playerid, string PlayerName, ushort MapId, ushort PosX, ushort PosY)
        {

        }
        public override void RequestParty(long Id, string Name)
        {
            
        }
        public override void InviteParty(long Id, string Name)
        {
            
        }
        public override void PartyStop(long Id, string Name)
        {
			
        }

        public override void AcceptParty(long Id, string Name)
        {
			
        }

        public override void NpcAppear(byte mode, ushort NpcIdInMap, ushort NpcId, string NpcName, ushort PosX, ushort PosY)
        {
            //ts.Debug("Mode: " + mode.ToString() + " - NPCIdInMap: " + NpcIdInMap.ToString());
            //ts.Debug("NPCName: " + NpcName + " - NpcId:" + NpcId + " - PosX: " + PosX + " - PosY: " + PosY);
            //ts.Debug("NpcIdInMap " + Newtonsoft.Json.JsonConvert.SerializeObject(NpcIdInMap));
            //ts.Debug("NpcId " + Newtonsoft.Json.JsonConvert.SerializeObject(NpcId));
            //ts.Debug("NpcName " + Newtonsoft.Json.JsonConvert.SerializeObject(NpcName));
            //ts.Debug("PosX " + Newtonsoft.Json.JsonConvert.SerializeObject(PosX));
            //ts.Debug("PosY " + Newtonsoft.Json.JsonConvert.SerializeObject(PosY));
        }
        public override void GroundItem(eGdThing kind, byte ItemIdInMap, ushort ItemId, string ItemName, ushort PosX, ushort PosY)
        {
            
        }

        public override void BattleStarted()
        {
			isBattle=true;
			CheckDisconnect();
            ts.DebugBattle(string.Format("-------------Bắt đầu trận đấu: {0}-------------", ts.TotalBattle));
            sw.Restart();
        }
        public override void BattleStopped()
        {
			
            sw.Stop();
            ts.DebugBattle(string.Format("-------------{0}-------------", sw.ElapsedMilliseconds));
            AutoEatFood();
            ProcessInventoryAction();
			isBattle=false;
            if (isBattleWaitting){
                isBattleWaitting = false;
            }
			EatGod();
			if (ts.Character.Name == ChuParty){
				foreach(var item in ListParty){          
					if(!item.Value){
						ts.InviteParty(item.Key);
					}
				}
			}else{
				if(!string.IsNullOrEmpty(ChuParty) && ts.TeamLeaderId == 0){
					ts.RequestParty(ChuParty);
				}
			}
        }

        public override void MyAttack(int CombatTurn)
        {
          
			if(isBoChayKhiKhongDuChuParty&& ts.TeamLeaderId == 0 && !string.IsNullOrEmpty(ChuParty)){
				if (ts.Character.Skills.Find(i => i.SkillId == 14002) != null){                  
                    ts.SendAttack(ts.Character.Col, ts.Character.Row, ts.Character.Col, ts.Character.Row, 14002);//đào tẩu
                }
                else{
                    ts.SendAttack(ts.Character.Col, ts.Character.Row, ts.Character.Col, ts.Character.Row, 18001);//chạy thường
                }
                return;
			}
			var m = findMonster();
			
			switch(CombatTurn){
				case 1:
					ts.SendAttack(ts.Character.Col, ts.Character.Row, m.Col, m.Row, 10000);
					break;
				case 2:
					ts.SendAttack(ts.Character.Col, ts.Character.Row, m.Col, m.Row, 10000);
					break;
				case 3:
					ts.SendAttack(ts.Character.Col, ts.Character.Row, m.Col, m.Row, 10000);
					break;
				default:break;
			}
        }
        public override void MyPartnerAttack(int CombatTurn)
        {
            var m = findMonster();
			switch(CombatTurn){
				case 1:
					ts.SendAttack(ts.CurrentPartner.Col, ts.CurrentPartner.Row, m.Col, m.Row, 10000);
					break;
				case 2:
					ts.SendAttack(ts.CurrentPartner.Col, ts.CurrentPartner.Row, m.Col, m.Row, 10000);
					break;
				case 3:
					ts.SendAttack(ts.CurrentPartner.Col, ts.CurrentPartner.Row, m.Col, m.Row, 10000);
					break;
				default:break;
			}
          
        }

        public override void NpcDialog(ushort Step, byte DialogType, int DialogId)
        {
            ts.Debug("[Dialog] Step = " + Step.ToString() + " - DialogType = " + DialogType.ToString() + " - DialogID = " + DialogId.ToString());
            //AutoResponseDialogType(DialogType,DialogId);
        }
        public override void GodChanged(bool isGod, byte value)
        {

        }
        public override void Tick10s()
        {
			
        }
		
        public override void ChannelFree(ushort ChannelId)
        {
			
        }

        public override void DungeonStart() { }
        public override void DungeonStop() { }
		