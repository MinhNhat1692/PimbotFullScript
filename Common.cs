		protected override NPCCombatObject findMonster()
        {

            var tmp = ts.oCombat.Values.ToList().FindAll(i => !i.isTeam && i.Hp > 0);
            if (tmp.Count > 0)
            {
                NPCCombatObject tmpCol = tmp[0];



                var listCol1 = tmp.FindAll(i => i.Col == tmpCol.Col);

                int MaxRow = listCol1.Max(i => i.Row);
                int MinRow = listCol1.Min(i => i.Row);
                int TBRrow = (MaxRow + MinRow) / 2;
                switch (listCol1.Count)
                {
                    case 5:
                        return listCol1.Find(i => i.Row == 1);
                    case 3:
                    case 4:
                        var tmpRow02 = listCol1.Find(i => i.Row == TBRrow);
                        if (tmpRow02 == null)
                        {
                            tmpRow02 = listCol1.Find(i => i.Row == TBRrow - 1);
                            if (tmpRow02 == null)
                            {
                                tmpRow02 = listCol1.Find(i => i.Row == TBRrow + 1);
                                if (tmpRow02 == null)
                                {
                                    tmpRow02 = listCol1.Find(i => i.Row == TBRrow - 2);
                                    if (tmpRow02 == null)
                                    {
                                        tmpRow02 = listCol1.Find(i => i.Row == TBRrow + 2);
                                        if (tmpRow02 == null)
                                        {
                                            return tmpCol;
                                        }
                                        else
                                        {
                                            return tmpRow02;
                                        }
                                    }
                                    else
                                    {
                                        return tmpRow02;
                                    }
                                }
                                else
                                {
                                    return tmpRow02;
                                }
                            }
                            else
                            {
                                return tmpRow02;
                            }
                        }
                        else
                        {
                            return tmpRow02;
                        }
                    default:
                        return tmpCol;
                }

            }

            return ts.oCombat.Values.ToList().FindAll(i => !i.isTeam)[0];

        }
        void doEatHP(byte PartnerIndex, int difHp)
        {
            var listHp = ts.listItemInBag.FindAll(i => i.Quantity > 0 && i.HP > 0).OrderBy(i => i.HP).ToList();

            for (byte i = 0; i < listHp.Count; i++)
            {
                var oSlot = listHp[i];
                int eatHpAmt = (int)((difHp - (difHp % oSlot.HP)) / oSlot.HP);
                if (eatHpAmt > 0)
                {
                    if (eatHpAmt > oSlot.Quantity)
                    {
                        eatHpAmt = oSlot.Quantity;
                    }
                    ts.EatItem(oSlot.Index, eatHpAmt, PartnerIndex);
                    difHp = difHp - eatHpAmt * oSlot.HP;
                }
            }
        }
        void doEatSP(byte PartnerIndex, int difSp)
        {
            var listSp = ts.listItemInBag.FindAll(i => i.Quantity > 0 && i.SP > 0).OrderBy(i => i.SP).ToList();

            for (byte i = 0; i < listSp.Count; i++)
            {
                var oSlot = listSp[i];
                int eatHpAmt = (int)((difSp - (difSp % oSlot.SP)) / oSlot.SP);
                if (eatHpAmt > 0)
                {
                    if (eatHpAmt > oSlot.Quantity)
                    {
                        eatHpAmt = oSlot.Quantity;
                    }
                    ts.EatItem(oSlot.Index, eatHpAmt, PartnerIndex);
                    difSp = difSp - eatHpAmt * oSlot.SP;
                }
            }
        }

        void AutoEatFood()
        {
            if (ts.Character.Hp < (ts.Character.MaxHp * hpFractionEat))
            {
                doEatHP(0, (int)((ts.Character.MaxHp * hpFraction) - ts.Character.Hp));
            }
            if (ts.Character.Sp < (ts.Character.MaxSp * spFractionEat))
            {
                doEatSP(0, (int)((ts.Character.MaxSp * spFraction) - ts.Character.Sp));
            }
            if (ts.CurrentPartner != null)
            {
                if (ts.CurrentPartner.Hp < (ts.CurrentPartner.MaxHp * hpFractionEat))
                {
                    doEatHP(ts.CurrentPartner.PetIndex, (int)((ts.CurrentPartner.MaxHp * hpFraction) - ts.CurrentPartner.Hp));
                }
                if (ts.CurrentPartner.Sp < (ts.CurrentPartner.MaxSp * spFractionEat))
                {
                    doEatSP(ts.CurrentPartner.PetIndex, (int)((ts.CurrentPartner.MaxSp * spFraction) - ts.CurrentPartner.Sp));
                }
            }
        }
        private void Disconnect(string msg, bool isStop = false)
        {
            ts.Debug(msg, 0x0000FF);
            ts.Disconnect(isStop);
        }
        void CheckDisconnect()
        {
            if (ts.Character != null && ts.Character.Hp < (DisconnectHpFlag * ts.Character.MaxHp))
            {
                Disconnect(string.Format("Disconnected : {0} HP {1} < {2}!!", ts.Character.Name, ts.Character.Hp, DisconnectHpFlag * ts.Character.MaxHp), true);
                return;
            }

            if (ts.CurrentPartner != null && ts.CurrentPartner.Hp < (DisconnectHpFlag * ts.CurrentPartner.MaxHp))
            {
                Disconnect(string.Format("Disconnected : {0} SP {1} < {2}!!", ts.CurrentPartner.Name, ts.CurrentPartner.Hp, DisconnectHpFlag * ts.CurrentPartner.MaxHp), true);
                //   Disconnect("Disconnected : Warrior HP is low !!");
                return;
            }
            if (ts.Character != null && ts.Character.Sp < (DisconnectSpFlag * ts.Character.MaxSp))
            {
                Disconnect(string.Format("Disconnected : {0} SP {1} < {2}!!", ts.Character.Name, ts.Character.Hp, DisconnectSpFlag * ts.Character.MaxSp), true);

                //      Disconnect("Disconnected : Character SP is low !!");
                return;
            }

            if (ts.CurrentPartner != null && ts.CurrentPartner.Sp < (DisconnectSpFlag * ts.CurrentPartner.MaxSp))
            {
                Disconnect(string.Format("Disconnected : {0} SP {1} < {2}!!", ts.CurrentPartner.Name, ts.CurrentPartner.Sp, DisconnectSpFlag * ts.CurrentPartner.MaxSp), true);
                return;
            }
            if (ts.CurrentPartner != null && ts.CurrentPartner.FAI < DisconFai)
            {
                Disconnect(string.Format("Disconnected : {0} FAI {1} < {2}!!", ts.CurrentPartner.Name, ts.CurrentPartner.FAI, DisconFai), true);
                return;
            }
            if (isDisInventory0HP)
            {
                var listSp = ts.listItemInBag.FindAll(i => i.PhanLoaiBag == EnumType.eThings.Bag && i.HP > 0 && i.Quantity > 0).ToList();
                if (listSp.Count == 0)
                {
                    Disconnect("Disconnected : KhÃ´ng cÃ²n HP trong tÃºi Ä‘á»“!!", true);
                    return;
                }
            }
            if (isDisInventory0SP)
            {
                var listSp = ts.listItemInBag.FindAll(i => i.PhanLoaiBag == EnumType.eThings.Bag && i.SP > 0 && i.Quantity > 0).ToList();
                if (listSp.Count == 0)
                {
                    Disconnect("Disconnected : KhÃ´ng cÃ²n SP trong tÃºi Ä‘á»“!!", true);
                }
            }
        }


         void ProcessInventoryAction()
        {
            var listSp = ts.listItemInBag.FindAll(i => DropItemList.Contains(i.ItemId) && i.Quantity > 0).ToList();
            listSp.ForEach(i => ts.DropItem(i.Index, i.Quantity));
            var listCon = ts.listItemInBag.FindAll(i => ContributeList.Contains(i.ItemId) && i.Quantity > 0).ToList();
            listCon.ForEach(i => ts.Contribute(i.Index));
        }
        void EatGod()
        {
            if (ts.Character.God == 0 && ts.Character.Ghost == 0)
            {
                var listGod = ts.listItemInBag.FindAll(i => i.SpecialAbility == 93 && i.Quantity > 0).OrderBy(i => i.SkillLink).ToList(); //TÃ¬m kiáº¿m tháº§n tÃ i, vÃ  sáº¯p xáº¿p theo thá»© tá»± sá»‘ lÆ°á»£ng nhá»� tÄƒng lÃªn
                if (listGod.Count > 0)
                {
                    ts.EatItem(listGod[0].Index, 1, 0);
                }
            }
        }
        
		