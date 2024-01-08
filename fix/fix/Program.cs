





using System.ComponentModel;

namespace fix
{
    public class Charater
    {
        public string Name { get; }
        public string Jop { get; }
        public int Level { get; }
        public int Atk { get; }
        public int Def { get; }
        public int Hp { get; }
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
            if(Count == 0)
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

            Console.WriteLine("");

            switch(CheckValidInput(1, 3))
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

            
            

            /*items.Add(new Item("수련자의 갑옷", "수련에 도움을 주는 갑옷입니다.", 0, 0, 5, 0, 1000));
            items.Add(new Item("무쇠갑옷", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 0, 9, 0, 350));
            items.Add(new Item("스파르타의 갑옷", "스파르타 전사들이 사용했다는 전설의 갑옷입니다.", 0, 0, 15, 0, 3500));
            items.Add(new Item("낡은 검", "쉽게 볼 수 있는 낡은 검 입니다.", 1, 2, 0, 0, 600));
            items.Add(new Item("청동 도끼", "어디선가 사용됐던거 같은 도끼입니다.", 1, 5, 0, 0, 1500));
            items.Add(new Item("스파르타의 창", "스파르타의 전사들이 사용했다는 전설의 창입니다.", 1, 7, 0, 0, 350));*/
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
