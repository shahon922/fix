
namespace fix
{
    internal class Program
    {
        public class Charater
        {
            public string Name { get; }
            public string Jop { get; }
            public int Level { get; }
            public int Atk { get; }
            public int Def { get; }
            public int Hp { get; }
            public int Gold { get; }

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

        class Item
        {
            

        }

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
        }

        private static void GameDataSetting()
        {
            
        }
    }
}
