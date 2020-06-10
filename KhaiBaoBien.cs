		List<ushort> DropItemList = new List<ushort>();
        List<ushort> ContributeList = new List<ushort>();
		Dictionary<string,bool> ListParty = new Dictionary<string,bool>();
		System.Diagnostics.Stopwatch sw;
		
		//dành cho tự động mua hpsp
		ushort mapTrain = 13884;
		string buyType = "";
		//nhớ set isDisInventory0HP và isDisInventory0SP = true ở dưới;
		
		//------HP-SP-Ngắt kết nối
        double DisconnectHpFlag = 0.3;    // Disconnect when current HP is below 30%
        double DisconnectSpFlag = 0.3;    // Disconnect when current SP is below 30%
        double hpFractionEat = 0.8; //Eat HP when current HP<= 80%
        double spFractionEat = 0.8;
        double hpFraction = 0.95;       //Eat until current HP >= 95 %
        double spFraction = 0.95;
        byte DisconFai = 00;		//	Faith of warrior to disconnect
        bool isDisInventory0HP = true;
        bool isDisInventory0SP = true;
		
		//Cấu hình train Level
		ushort PosXAnToan=0;
		ushort PosYAnToan=0;
		ushort PosXTrain=0;
		ushort PosYTrain=0;
		ushort NpcIdTruyKich =0;
		bool isBoChayKhiKhongDuChuParty=false;
		bool isDisconnectWhenMemberLeave=false;
		//-------------------- Party
		string ChuParty="MIP";
		string QuanSu="";
		string ThanhVien1="";
		string ThanhVien2="";
		string ThanhVien3="";
		
		private void AddParty(string Name){
			if(!string.IsNullOrEmpty(Name) && !ListParty.ContainsKey(Name)){
				ListParty.Add(Name,false);
			}
		}
		private bool isFullParty(){
			foreach(var item in ListParty)
            {
                if(!item.Value){
					return false;
				}
            }
			return true;
		}
		private bool isMember(string Name){
			foreach(var item in ListParty)
            {
                if(item.Key==Name){
					return true;
				}
            }
			return false;
		}