﻿





using System.ComponentModel;

namespace fix
{
    public class Charater
    {
        public string Name { get; }
        public string Jop { get; }
        public int Level { get; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }

        public Charater(string name, string jop, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Jop = jop;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;

        }
    }

    public class Monster
    {
        public string Name { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }

        public Monster(string name,int def, int hp)
        {
            Name = name;
            Def = def;
            Hp = hp;
        }
    }

    public class Item
    {
        public string Name { get; }
        public string Description { get; }
        public int Type { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
        public int Price { get; set; }
        public bool IsEquipped { get; set; }
        public static int ItemCnt = 0;
        public int Count = 1; // 아이템 개수

        public Item(string name, string description, int type, int atk, int def, int hp,int price, bool isEquipped = false, int count = 1)
        {
            Name = name;
            Description = description;
            Type = type;
            Atk = atk;
            Def = def;
            Hp = hp;
            Price = price;
            IsEquipped = isEquipped;
            Count = count;
        }

        public void PrintItmeStatDescription(bool withNumber = false,int idx = 0) // 인벤토리 장착 E 출력
        {
            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0} ", idx);
                Console.ResetColor();
            }
            if (IsEquipped)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("E");
                Console.ResetColor();
                Console.Write("]");
                Console.Write(PadRightForMixedText(Name, 9));
            }
            else
            {
                Console.Write(PadRightForMixedText(Name, 12));
            }
            Console.Write(" | ");

            // {(Atk >= 0 ? "+" : "") [조건 ? 조건이 참이라면 : 조건이 거짓이라면]
            if (Atk != 0)
            {
                Console.Write($"Atk {(Atk >= 0 ? "+" : "")}{Atk}");
            }
            else if (Def != 0)
            {
                Console.Write($"Def {(Def >= 0 ? "+" : "")}{Def}");
            }
            else if (Hp != 0)
            {
                Console.Write($"Hp {(Hp >= 0 ? "+" : "")}{Hp}");
            }

            Console.Write(" | ");

            Console.WriteLine(Description);

        }

        public void PurchasItmeDescription(bool withNumber = false, int idx = 0) // 상점 내용 출력
        {
            Console.Write("- ");
            if (withNumber)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.Write("{0} ", idx);
                Console.ResetColor();
            }         
            Console.Write(PadRightForMixedText(Name, 9));
            Console.Write(" | ");

            // {(Atk >= 0 ? "+" : "") [조건 ? 조건이 참이라면 : 조건이 거짓이라면]
            // 능력치가 0이 아니라면 +를 붙여 출력해라 : 붙이지 말고 출력해라
            if (Atk != 0)
            {
                Console.Write($"Atk {(Atk >= 0 ? "+" : "")}{Atk}");
            }
            else if (Def != 0)
            {
                Console.Write($"Def {(Def >= 0 ? "+" : "")}{Def}");
            }
            else if (Hp != 0)
            {
                Console.Write($"Hp {(Hp >= 0 ? "+" : "")}{Hp}");
            }

            Console.Write(" | ");

            Console.Write(Description);

            Console.Write(" | ");

            if(Count == 1)
            {
                Console.WriteLine("{0} G", Price);
            }
            else
            {
                Console.WriteLine("구매완료");
            }

            
        }


        public static int GetPrintableLength(string str)
        {
            int length = 0;
            foreach (char c in str)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    length += 2; // 한글과 같은 넓은 문자에 대해 길이를 2로 취급
                }
                else
                {
                    length += 1; // 나머지 문자에 대해 길이를 1로 취급
                }
            }

            return length;

        }

        public static string PadRightForMixedText(string str, int totalLength)
        {
            int currentLength = GetPrintableLength(str);
            int padding = totalLength - currentLength;
            return str.PadRight(str.Length + padding);
        }

        
    }
    internal class Program
    {
        static Charater player;
        static Monster[] monster;
        static Item[] items; // 상점 아이템 목록
        static Item[] inven = new Item[10]; // 인벤토리 목록
        static int invenIdx = 0;
        static int shopFailed;
        static void Main(string[] args)
        {
            // 구성
            // 0. 데이터 초기화
            // 1. 스타팅 로고 보여줌
            // 2. 선택화면 보여줌(상태창/인벤토리)
            // 3. 상태창을 구현(캐릭터/아이템)
            // 4. 인벤토리 화면 구현
            // 4-1 장착화면 구현
            // 5. 선택화면 확장
            GameDataSetting();
            StartMenu();
            
        }

        static void StartMenu()
        {
            //구성
            //0. 화면정리
            //1. 선택멘트 줌
            //2. 선택 결과값 검증함
            //3. 결과에 따라 메뉴로 보냄
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");

            Console.WriteLine("");

            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전입장");
            Console.WriteLine("5. 휴식하기");

            Console.WriteLine("");

            switch(CheckValidInput(1, 5))
            {
                case 1:
                    StatusMenu();
                    break;
                case 2:
                    InventoryMenu();
                    break;
                case 3:
                    ShopMenu();
                    break;
                case 4:
                    DungeonMenu();
                    break;
                case 5:
                    Rest();
                    break;

            }
            
        }

        private static void Rest()
        {
            if (player.Hp == 100) 
            {
                Console.WriteLine("이미 체력이 가득 차 있습니다.");
            }
            else
            {
                if (player.Gold >= 500) 
                {
                    Console.WriteLine("체력을 회복했습니다.");
                    player.Hp = 100;
                    player.Gold -= 500;
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                }
            }

            Console.WriteLine("");

            Console.WriteLine("0. 나가기");

            Console.WriteLine("");

            switch (CheckValidInput(0, 0))
            {
                case 0:
                    StartMenu();
                    break;
                default:
                    Rest();
                    break;

            }
        }

        private static void DungeonMenu()
        {
            Console.Clear();

            ShowHighlightedText("던전입장");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine("");


            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 쉬운 던전");
            Console.WriteLine("2. 일반 던전");
            Console.WriteLine("3. 어려운 던전");

            Console.WriteLine("");

            switch (CheckValidInput(0, 3))
            {
                case 0:
                    StartMenu();
                    break;
                case 1:
                    Easy();
                    break;
                case 2:
                    Normal();
                    break;
                case 3:
                    Hard();
                    break;

            }
        }

        private static void Easy()
        {
            Console.Clear();
            if (player.Hp <= 0) // 체력이 0이하 못함
            {
                Console.WriteLine("체력이 없습니다.");
            }
            else
            {
                monster[0] = new Monster("slime", 5, 50); // 쉬움

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Easy(쉬움)");
                Console.ResetColor();

                while (player.Hp > 0 && monster[0].Hp > 0)
                {
                    Random damage = new Random(); // 랜덤으로 숫자가 만들어 지도록
                    int monster_damage = damage.Next(10, 41); // 유저의 공격력 이상 41미만 중 숫자 출력
                    Console.WriteLine("{0}이 공격했습니다. {1} {2} 데미지", player.Name, monster[0].Name, player.Atk);
                    monster[0].Hp -= monster_damage; // 몬스터의 체력을 데미지만큼 빼고 넣음

                    Console.WriteLine("");

                    int player_damage = damage.Next(20, 36); // 몬스터의 공격 데미지 20이상 36미만 중 숫자 출력
                    Console.WriteLine("{0}이 공격했습니다. {1} {2} 데미지", monster[0].Name, player.Name, player_damage);
                    player.Hp -= player_damage; // 유저의 체력을 데미지 만큼 빼고 넣음
                    Console.WriteLine("");

                    if (monster[0].Hp <= 0) //몬스터의 체력이 0보다 작을 때
                    {

                        ShowHighlightedText("던전 클리어");
                        Console.WriteLine("축하합니다!!");
                        Console.WriteLine("쉬운 던전을 클리어 하였습니다.");

                        Console.WriteLine("[탐험 결과]");
                        Console.Write("체력 : ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0}", player.Hp);
                        Console.ResetColor();

                        player.Gold += 1000;
                        Console.Write("Gold : ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("{0} ", player.Gold);
                        Console.ResetColor();
                        Console.WriteLine("G");

                        Console.WriteLine("");

                    }
                    else if (player.Hp <= 0)
                    {
                        Console.WriteLine("{0}의 체력이 0이 되었습니다.", player.Name);
                        Console.WriteLine("던전 실패");
                    }
                }
            }

            Console.WriteLine("");

            Console.WriteLine("0. 나가기");

            Console.WriteLine("");

            switch (CheckValidInput(0, 0))
            {
                case 0:
                    DungeonMenu();
                    break;
                default:
                    Easy();
                    break;

            }
        }

        private static void Normal()
        {
            Console.Clear();
            if (player.Hp <= 0)
            {
                Console.WriteLine("체력이 없습니다.");
            }
            else
            {
                monster[1] = new Monster("goblin", 7, 70); // 보통

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Normal(보통)");
                Console.ResetColor();

                while (player.Hp > 0 && monster[1].Hp > 0)
                {
                    Random damage = new Random(); // 랜덤으로 숫자가 만들어 지도록
                    int monster_damage = damage.Next(10, 41); // 유저의 공격력 이상 41미만 중 숫자 출력
                    Console.WriteLine("{0}이 공격했습니다. {1} {2} 데미지", player.Name, monster[1].Name, player.Atk);
                    monster[1].Hp -= monster_damage; // 몬스터의 체력을 데미지만큼 빼고 넣음


                    Console.WriteLine("");
                    int player_damage = damage.Next(20, 36); // 몬스터의 공격 데미지 20이상 36미만 중 숫자 출력
                    Console.WriteLine("{0}이 공격했습니다. {1} {2} 데미지", monster[1].Name, player.Name, player_damage);
                    player.Hp -= player_damage; // 유저의 체력을 데미지 만큼 빼고 넣음
                    Console.WriteLine("");

                    if (monster[0].Hp <= 0) //몬스터의 체력이 0보다 작을 때
                    {

                        ShowHighlightedText("던전 클리어");
                        Console.WriteLine("축하합니다!!");
                        Console.WriteLine("일반 던전을 클리어 하였습니다.");

                        Console.WriteLine("[탐험 결과]");
                        Console.Write("체력 : ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0}", player.Hp);
                        Console.ResetColor();

                        player.Gold += 1700;
                        Console.Write("Gold : ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("{0} ", player.Gold);
                        Console.ResetColor();
                        Console.WriteLine("G");

                        Console.WriteLine("");

                    }
                    else if (player.Hp <= 0)
                    {
                        Console.WriteLine("{0}의 체력이 0이 되었습니다.", player.Name);
                        Console.WriteLine("던전 실패");
                    }
                }
            }
   
            Console.WriteLine("");

            Console.WriteLine("0. 나가기");

            Console.WriteLine("");

            switch (CheckValidInput(0, 0))
            {
                case 0:
                    DungeonMenu();
                    break;
                default:
                    Normal();
                    break;

            }
        }

        private static void Hard()
        {
            Console.Clear();
            if(player.Hp <= 0)
            {
                Console.WriteLine("체력이 없습니다.");
            }
            else
            {
                monster[2] = new Monster("wolf", 11, 100); // 어려움

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Hard(어려움)");
                Console.ResetColor();

                while (player.Hp > 0 && monster[2].Hp > 0)
                {
                    Random damage = new Random(); // 랜덤으로 숫자가 만들어 지도록
                    int monster_damage = damage.Next(10, 41); // 유저의 공격력 이상 41미만 중 숫자 출력
                    Console.WriteLine("{0}이 공격했습니다. {1} {2} 데미지", player.Name, monster[2].Name, player.Atk);
                    monster[2].Hp -= monster_damage; // 몬스터의 체력을 데미지만큼 빼고 넣음

                    Console.WriteLine("");

                    int player_damage = damage.Next(20, 36); // 몬스터의 공격 데미지 20이상 36미만 중 숫자 출력
                    Console.WriteLine("{0}이 공격했습니다. {1} {2} 데미지", monster[2].Name, player.Name, player_damage);
                    player.Hp -= player_damage; // 유저의 체력을 데미지 만큼 빼고 넣음
                    Console.WriteLine("");

                    if (player.Hp <= 0)
                    {
                        Console.WriteLine("{0}의 체력이 0이 되었습니다.", player.Name);
                        Console.WriteLine("던전 실패");
                    }
                    else if (monster[2].Hp <= 0) //몬스터의 체력이 0보다 작을 때
                    {

                        ShowHighlightedText("던전 클리어");
                        Console.WriteLine("축하합니다!!");
                        Console.WriteLine("어려운 던전을 클리어 하였습니다.");

                        Console.WriteLine("[탐험 결과]");
                        Console.Write("체력 : ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0}", player.Hp);
                        Console.ResetColor();

                        player.Gold += 2500;
                        Console.Write("Gold : ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("{0} ", player.Gold);
                        Console.ResetColor();
                        Console.WriteLine("G");

                        Console.WriteLine("");

                    }
                }
            }
        
            Console.WriteLine("");

            Console.WriteLine("0. 나가기");

            Console.WriteLine("");

            switch (CheckValidInput(0, 0))
            {
                case 0:
                    DungeonMenu();
                    break;
                default:
                    Hard();
                    break;

            }
        }

        private static void PurchaseMenu()
        {
            Console.Clear();

            ShowHighlightedText("상점 - 아이템 구매");
            Console.WriteLine("필요한 아이템을 얻을 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("{0} ", player.Gold);
            Console.ResetColor();
            Console.WriteLine("G");


            for (int i = 0; i < Item.ItemCnt; i++)
            {
                items[i].PurchasItmeDescription(true, i + 1);
            }

            Console.WriteLine("");
            if (shopFailed == -1)
            {
                Console.WriteLine("Gold가 부족합니다.");
            }
            else if(shopFailed == -2)
            {
                Console.WriteLine("이미 구매한 상품입니다.");
            }

            Console.WriteLine("");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");

            int keyInput = CheckValidInput(0, Item.ItemCnt);

            switch (keyInput)
            {
                case 0:
                    ShopMenu();
                    break;
                default:
                    BuyItem(keyInput - 1);
                    PurchaseMenu();
                    break;

            }

        }

        private static void ShopMenu()
        {
            Console.Clear();

            ShowHighlightedText("상점");
            Console.WriteLine("필요한 아이템을 얻을 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("{0} ", player.Gold);
            Console.ResetColor();
            Console.WriteLine("G");

            for (int i = 0; i < Item.ItemCnt; i++)
            {
                items[i].PurchasItmeDescription();
            }
            Console.WriteLine("");

            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 아이템 구매");

            Console.WriteLine("");

            switch (CheckValidInput(0, 1))
            {
                case 0:
                    StartMenu();
                    break;
                case 1:
                    PurchaseMenu();
                    break;

            }
        }

        private static void InventoryMenu()
        {
            Console.Clear();

            ShowHighlightedText("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");

            for(int i = 0; i < invenIdx; i++)
            {
                inven[i].PrintItmeStatDescription();
            }
            Console.WriteLine("");

            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 장착관리");

            Console.WriteLine("");

            switch (CheckValidInput(0, 1))
            {
                case 0:
                    StartMenu();
                    break;
                case 1:
                    EquipMenu();
                    break;

            }
        }

      


        private static void EquipMenu()
        {
            Console.Clear();

            ShowHighlightedText("인벤토리 - 장착관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");

            for (int i = 0; i < invenIdx; i++)
            {
                inven[i].PrintItmeStatDescription(true, i + 1);
            }

            
            Console.WriteLine("");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");

            int keyInput = CheckValidInput(0, invenIdx); //Item.ItemCnt);

            switch (keyInput)
            {
                case 0:
                    InventoryMenu();
                    break;
                default:
                    ToggleEquipStatus(keyInput - 1);
                    EquipMenu();
                    break;

            }
        }

        private static void ToggleEquipStatus(int idx)
        {
            //items[idx].IsEquipped = !items[idx].IsEquipped;
            inven[idx].IsEquipped = !inven[idx].IsEquipped;
        }


        private static int BuyItem(int idx)
        {
            if (items[idx].Count != 0)
            {
                if (player.Gold >= items[idx].Price)
                {
                    player.Gold -= items[idx].Price; // gold에서 가격을 뺌
                    Array.Copy(items, idx, inven, invenIdx++, 1); // items에 있는 내용을 인벤토리에 복사
                    items[idx].Count = 0;

                }
                else if (player.Gold < items[idx].Price)
                {
                    return shopFailed = -1;
                }
            }
            else
            {
                return shopFailed = -2;
            }
            return player.Gold;
        }

        private static void StatusMenu()
        {
            Console.Clear();

            ShowHighlightedText("상태 보기");
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");

            PrintTextWithHighlights("Lv. ", player.Level.ToString("00"));
            Console.WriteLine("");
            Console.WriteLine("{0} ( {1} )", player.Name, player.Jop);

            int bonusAtk = GetSumBonusAtk();
            int bonusDef = GetSumBonusDef();
            int bonusHp = GetSumBonusHp();
            PrintTextWithHighlights("공격력 : ", (player.Atk + bonusAtk).ToString(), bonusAtk > 0 ? string.Format(" (+{0})", bonusAtk) : "");
            PrintTextWithHighlights("방어력 : ", (player.Def + bonusDef).ToString(), bonusDef > 0 ? string.Format(" (+{0})", bonusDef) : "");
            PrintTextWithHighlights("체 력 : ", (player.Hp + bonusHp).ToString(), bonusHp > 0 ? string.Format(" (+{0})", bonusHp) : "");
            PrintTextWithHighlights("골 드 : ", player.Gold.ToString());
            Console.WriteLine("");
            Console.WriteLine("0. 뒤로가기");
            Console.WriteLine("");
            switch (CheckValidInput(0, 0))
            {
                case 0:
                    StartMenu();
                    break;
                

            }
        }

        private static int GetSumBonusAtk()
        {
            int sum = 0;
            for(int i = 0; i < Item.ItemCnt; i++)
            {
                if (items[i].IsEquipped)
                {
                    sum += items[i].Atk;
                }
            }
            return sum;
        }



        private static int GetSumBonusDef()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (items[i].IsEquipped)
                {
                    sum += items[i].Def;
                }
            }
            return sum;
        }

        private static int GetSumBonusHp()
        {
            int sum = 0;
            for (int i = 0; i < Item.ItemCnt; i++)
            {
                if (items[i].IsEquipped)
                {
                    sum += items[i].Hp;
                }
            }
            return sum;
        }

        private static void ShowHighlightedText(string text)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        private static void PrintTextWithHighlights(string s1, string s2, string s3 = "")
        {
            Console.Write(s1);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(s2);
            Console.ResetColor();
            Console.WriteLine(s3);
        }

        private static int CheckValidInput(int min, int max)
        {
            // 설명
            // 아래 두 가지 상황은 비정상 -> 재입력 수행
            // 1. 숫자가 아닌 입력을 받은 경우
            // 2. 숫자가 최소값-최대값의 범위를 넘는 경우
            int keyInput; //tryParse
            bool result; //while

            do
            {
                Console.WriteLine("원하시는 행동을 입력해주세요");
                Console.Write(">>");
                result = int.TryParse(Console.ReadLine(), out keyInput);
            } while (result == false || CheckIfValid(keyInput, min, max) == false);

            return keyInput;
        }

        private static bool CheckIfValid(int keyInput, int min, int max)
        {
            if(min <= keyInput && keyInput <= max)
            {
                return true;
            }
            else
            { 
                return false;
            }

        }

        

        private static void GameDataSetting()
        {
            player = new Charater("chad", "전사", 1, 10, 5, 100, 1500);
            //items = new List<Item>();
            items = new Item[10];                
            AddItem(new Item("수련자의 갑옷", "수련에 도움을 주는 갑옷입니다.", 0, 0, 5, 0, 1000));
            AddItem(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 9, 0, 350));
            AddItem(new Item("스파르타의 갑옷", "스파르타 전사들이 사용했다는 전설의 갑옷입니다.", 0, 0, 15, 0, 3500));
            AddItem(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 1, 2, 0, 0, 600));
            AddItem(new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 1, 5, 0, 0, 1500));
            AddItem(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 1, 7, 0, 0, 350));

            monster = new Monster[3];

            
        }

        static void AddItem(Item item)
        {
            if(Item.ItemCnt == 10)
            {
                return;
            }
            items[Item.ItemCnt] = item;
            Item.ItemCnt++;
        }

    }
}
