namespace TextGamePorject
{
    internal class Program
    {
        enum Select
        {
            상태보기 = 1,
            인벤토리,
            상점
        }

        class Item
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsEquipped { get; set; }

            public Item(string name, string description)
            {
                Name = name;
                Description = description;
                IsEquipped = false; // 처음에는 장착되지 않은 상태
            }
        }

        //보유 중인 아이템
        static List<Item> inventory = new List<Item>();


        // 게임에서 존재하는 아이템
        static List<Item> itemList = new List<Item>
        {
            new Item("수련자 갑옷","방어력 +5 | 수련에 도움을 주는 갑옷입니다."),
            new Item("무쇠갑옷", "방어력 +9 | 무쇠로 만들어져 튼튼한 갑옷입니다."),
            new Item("스파르타의 갑옷", "방어력 +15 | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다."),
            new Item("낡은 검", "공격력 +2 | 쉽게 볼 수 있는 낡은 검입니다."),
            new Item("청동 도끼", "공격력 +5 | 어디선가 사용됐던거 같은 도끼입니다."),
            new Item("스파르타의 창", "공격력 +7 | 스파르타의 전사들이 사용했다는 전설의 창입니다."),


        };



        static void Main(string[] args)
        {
            inventory.Add(new Item("무쇠갑옷", "방어력 +9 | 무쇠로 만들어져 튼튼한 갑옷입니다."));
            inventory.Add(new Item("스파르타의 창", "공격력 +7 | 스파르타의 전사들이 사용했다는 전설의 창입니다."));
            inventory.Add(new Item("낡은 검", "공격력 +2 | 쉽게 볼 수 있는 낡은 검입니다."));

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.\n이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine("");
                Console.WriteLine($"1. {Select.상태보기}");
                Console.WriteLine($"2. {Select.인벤토리}");
                Console.WriteLine($"3. {Select.상점}");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");

                string input = Console.ReadLine();

                if (Enum.TryParse<Select>(input, out Select selectedAction))
                {
                    switch (selectedAction)
                    {
                        case Select.상태보기:
                            ShowStat();
                            break;
                        case Select.인벤토리:
                            ShowInven();
                            break;
                        case Select.상점:
                            ShowShop();
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

            }
        }
        static void BuyItem(int gold, Dictionary<Item, int> shopItems)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("상점 - 아이템 구매");
            Console.ResetColor();
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine("");
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{gold} G");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");

            // 상점 아이템 목록 출력
            int index = 1;
            foreach (var item in shopItems)
            {
                if (item.Value != -1) // 구매완료된 아이템은 제외
                {
                    int itemNamePadding = 25 - item.Key.Name.Length;
                    int itemDescPadding = 55 - item.Key.Description.Length;
                    Console.WriteLine($"- {index}. {item.Key.Name}{new string(' ', itemNamePadding)} | {item.Key.Description}{new string(' ', itemDescPadding)} | {item.Value,-6} G");
                    index++;
                }
            }
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");

            string input = Console.ReadLine();
            if (input == "0")
            {
                return;
            }
            else if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex < index)
            {
                Item selected = shopItems.Keys.ElementAt(selectedIndex - 1);
                int price = shopItems[selected];
                if (price == -1)
                {
                    Console.WriteLine("이미 구매한 아이템입니다.");
                }
                else if (price <= gold)
                {
                    gold -= price;
                    inventory.Add(selected); // 인벤토리에 아이템 추가
                    shopItems[selected] = -1; // 구매 완료 표시
                    Console.WriteLine($"'{selected.Name}' 아이템을 구매했습니다.");
                    Console.WriteLine($"현재 보유 골드: {gold} G");
                }
                else
                {
                    Console.WriteLine("Gold가 부족합니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }
        static void ShowStat()
        {
            int baseAttack = 10;
            int baseDefense = 5;
            int baseHealth = 100;
            int currentAttack = baseAttack;
            int currentDefense = baseDefense;
            int currentHealth = baseHealth;
            // 아이템을 장착한 경우 해당 아이템의 공격력과 방어력을 반영
            foreach (var item in inventory)
            {
                if (item.IsEquipped)
                {
                    if (item.Description.Contains("공격력"))
                    {
                        string[] parts = item.Description.Split(new string[] { "공격력 +" }, StringSplitOptions.None);
                        currentAttack += int.Parse(parts[1].Split(' ')[0]);
                    }
                    else if (item.Description.Contains("방어력"))
                    {
                        string[] parts = item.Description.Split(new string[] { "방어력 +" }, StringSplitOptions.None);
                        currentDefense += int.Parse(parts[1].Split(' ')[0]);
                    }
                }
            }


            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("상태 보기");
            Console.ResetColor();
            Console.WriteLine("캐릭터의 정보가 표시됩니다.");
            Console.WriteLine("Lv. 01");
            Console.WriteLine("Chad ( 전사 )");
            Console.WriteLine($"공격력 : {currentAttack} (+{currentAttack - baseAttack})");
            Console.WriteLine($"방어력 : {currentDefense} (+{currentDefense - baseDefense})");
            Console.WriteLine("체 력 : 100");
            Console.WriteLine("Gold : 1500 G");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");

            string input = Console.ReadLine();
            if (input == "0")
            {
                Console.Clear();
                return;
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        static void ShowInven()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("인벤토리");
            Console.ResetColor();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            foreach (var item in inventory)
            {
                if (item.IsEquipped)
                {
                    Console.WriteLine($"- [E]{item.Name}\t| {item.Description}");
                }
                else
                {
                    Console.WriteLine($"- {item.Name}\t| {item.Description}");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");

            string input = Console.ReadLine();
            if (input == "0")
            {
                Console.Clear();
                return;
            }
            else if (input == "1") // 아이템 장착 관리 기능 추가
            {
                ShowEquipManagement();
            }

        }
        static void ShowEquipManagement()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.ResetColor();
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine("");
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < inventory.Count; i++)
            {
                var item = inventory[i];
                if (item.IsEquipped)
                {
                    Console.WriteLine($"[{i + 1}] [E]{item.Name}\t| {item.Description}");
                }
                else
                {
                    Console.WriteLine($"[{i + 1}] {item.Name}\t| {item.Description}");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("0. 나가기");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");
            string input = Console.ReadLine();
            if (input == "0")
            {
                Console.Clear();
                return;
            }
            else if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= inventory.Count)
            {
                Item selected = inventory[selectedIndex - 1];
                if (selected.IsEquipped)
                {
                    selected.IsEquipped = false;
                    Console.WriteLine($"'{selected.Name}' 아이템을 장착 해제했습니다.");
                }
                else
                {
                    selected.IsEquipped = true;
                    Console.WriteLine($"'{selected.Name}' 아이템을 장착했습니다.");
                }
                ShowEquipManagement(); // 다시 장착 관리 기능 호출
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        static void ShowShop()
        {
            int gold = 800; // 보유 골드
                            // 상점 아이템 목록
            Dictionary<Item, int> shopItems = new Dictionary<Item, int>
            {
                { new Item("수련자 갑옷", "방어력 +5 | 수련에 도움을 주는 갑옷입니다."), 1000 },
                { new Item("무쇠갑옷", "방어력 +9 | 무쇠로 만들어져 튼튼한 갑옷입니다."), -1 },
                { new Item("스파르타의 갑옷", "방어력 +15 | 스파르타의 전사들이 사용했다는 전설의 갑옷입니다."), 3500 },
                { new Item("낡은 검", "공격력 +2 | 쉽게 볼 수 있는 낡은 검입니다."), 600 },
                { new Item("청동 도끼", "공격력 +5 | 어디선가 사용됐던거 같은 도끼입니다."), 1500 },
                { new Item("스파르타의 창", "공격력 +7 | 스파르타의 전사들이 사용했다는 전설의 창입니다."), -1 }
            };
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("상점");
                Console.ResetColor();
                Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
                Console.WriteLine("");
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{gold} G");
                Console.WriteLine("");
                Console.WriteLine("[아이템 목록]");
                foreach (var item in shopItems)
                {
                    if (item.Value == -1)
                    {
                        Console.WriteLine($"- {item.Key.Name,-20} | {item.Key.Description,-50} | 구매완료");
                    }
                    else
                    {
                        Console.WriteLine($"- {item.Key.Name,-20} | {item.Key.Description,-50} | {item.Value,-10} G");
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("원하시는 행동을 입력해주세요.\n>>");

                string input = Console.ReadLine();
                if (input == "0")
                {
                    Console.Clear();
                    break;
                }
                else if (input == "1")
                {
                    BuyItem(gold, shopItems);
                    ShowShop();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ReadKey(); // 사용자 입력 대기
                }
            }

        }
    }
}