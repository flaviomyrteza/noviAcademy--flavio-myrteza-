using WorldRank;

var playerRepo = new InMemoryPlayerRepository();
IWalletRepository walletRepo = new InMemoryWalletRepository();

while (true)
{
    Console.WriteLine("\n=== WorldRank Player Registry ===");
    Console.WriteLine("1. Add player");
    Console.WriteLine("2. List all players");
    Console.WriteLine("3. Find player by name");
    Console.WriteLine("4. Group players by score");
    Console.WriteLine("5. Add wallet to player");
    Console.WriteLine("6. List wallets for player");
    Console.WriteLine("7. Deposit into wallet");
    Console.WriteLine("8. Withdraw from wallet");
    Console.WriteLine("9. Block / unblock wallet");
    Console.WriteLine("10. Filter & sort players (LINQ)");
    Console.WriteLine("0. Exit");
    Console.Write("> ");

    Action? action = Console.ReadLine() switch
    {
        "1" => AddPlayer,
        "2" => ListPlayers,
        "3" => FindPlayer,
        "4" => GroupByScore,
        "5" => AddWallet,
        "6" => ListWallets,
        "7" => Deposit,
        "8" => Withdraw,
        "9" => ToggleBlock,
        "10" => FilterAndSortPlayers,
        "0" => null,
        _ => () => Console.WriteLine("Unknown option.")
    };

    if (action is null)
        return; // "0" selected — exit

    try
    {
        action();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

void AddPlayer()
{
    Console.Write("Name: ");
    var name = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("Name cannot be empty.");
        return;
    }

    Console.Write("Score: ");
    var scoreInput = Console.ReadLine();
    if (!int.TryParse(scoreInput, out var score))
    {
        Console.WriteLine("Score must be a whole number.");
        return;
    }

    var player = new Player(name);
    player.UpdateScore(score);

    playerRepo.AddPlayer(player);
    Console.WriteLine($"Player added successfully. Id: {player.Id}");
}

void ListPlayers()
{
    var players = playerRepo.GetAll().ToList();
    if (players.Count == 0)
    {
        Console.WriteLine("No players registered.");
        return;
    }

    foreach (var p in players)
        Console.WriteLine(p);
}

void FindPlayer()
{
    Console.Write("Search by name: ");
    var term = Console.ReadLine() ?? string.Empty;

    var player = playerRepo.GetAll()
            .FirstOrDefault(p => p.Name.Equals(term, StringComparison.OrdinalIgnoreCase));

    if (player is null)
    {
        Console.WriteLine("No player found.");
        return;
    }

    Console.WriteLine(player);
}

void GroupByScore()
{
    foreach (var group in playerRepo.GroupPlayersByScore().OrderByDescending(g => g.Key))
    {
        Console.WriteLine($"Score {group.Key}:");
        foreach (var p in group)
            Console.WriteLine($"   - {p.Name}");
    }
}

Player? FindPlayerByIdPrompt()
{
    Console.Write("Player Id (Guid): ");
    var input = Console.ReadLine();
    if (!Guid.TryParse(input, out var id))
    {
        Console.WriteLine("Invalid Id format.");
        return null;
    }

    var player = playerRepo.FindPlayer(id);
    if (player is null)
        Console.WriteLine("Player not found.");

    return player;
}

void AddWallet()
{
    var player = FindPlayerByIdPrompt();
    if (player is null) return;

    Console.Write($"Currency ({string.Join("/", Enum.GetNames<Currency>())}): ");
    if (!Enum.TryParse<Currency>(Console.ReadLine(), ignoreCase: true, out var currency))
    {
        Console.WriteLine("Invalid currency.");
        return;
    }

    Console.Write("Opening balance: ");
    if (!decimal.TryParse(Console.ReadLine(), out var opening))
    {
        Console.WriteLine("Invalid amount.");
        return;
    }

    var wallet = new Wallet(currency, opening);
    player.AttachWallet(wallet);
    walletRepo.Add(wallet, player.Id);

    Console.WriteLine("Wallet added.");
}

void ListWallets()
{
    var player = FindPlayerByIdPrompt();
    if (player is null) return;

    var wallets = walletRepo.GetByPlayer(player.Id).ToList();
    if (wallets.Count == 0)
    {
        Console.WriteLine("No wallets for this player.");
        return;
    }

    foreach (var w in wallets)
        Console.WriteLine(w);
}

Wallet? SelectWallet()
{
    var player = FindPlayerByIdPrompt();
    if (player is null) return null;

    var wallets = walletRepo.GetByPlayer(player.Id).ToList();
    if (wallets.Count == 0)
    {
        Console.WriteLine("No wallets for this player.");
        return null;
    }

    Console.Write($"Currency ({string.Join("/", wallets.Select(w => w.Currency))}): ");
    if (!Enum.TryParse<Currency>(Console.ReadLine(), ignoreCase: true, out var currency))
    {
        Console.WriteLine("Invalid currency.");
        return null;
    }

    var wallet = wallets.FirstOrDefault(w => w.Currency == currency);
    if (wallet is null)
        Console.WriteLine("Wallet not found.");

    return wallet;
}

void Deposit()
{
    var wallet = SelectWallet();
    if (wallet is null) return;

    Console.Write("Amount: ");
    if (!decimal.TryParse(Console.ReadLine(), out var amount))
    {
        Console.WriteLine("Invalid amount.");
        return;
    }

    wallet.Deposit(amount);
    Console.WriteLine($"New balance: {wallet.Balance:0.00}");
}

void Withdraw()
{
    var wallet = SelectWallet();
    if (wallet is null) return;

    Console.Write("Amount: ");
    if (!decimal.TryParse(Console.ReadLine(), out var amount))
    {
        Console.WriteLine("Invalid amount.");
        return;
    }

    wallet.Withdraw(amount);
    Console.WriteLine($"New balance: {wallet.Balance:0.00}");
}

void ToggleBlock()
{
    var wallet = SelectWallet();
    if (wallet is null) return;

    if (wallet.IsBlocked) wallet.Unblock();
    else wallet.Block();

    Console.WriteLine($"Wallet is now {(wallet.IsBlocked ? "blocked" : "unblocked")}.");
}

void FilterAndSortPlayers()
{
    Console.Write("Minimum score: ");
    if (!int.TryParse(Console.ReadLine(), out var minScore))
    {
        Console.WriteLine("Invalid score.");
        return;
    }

    var results = playerRepo.GetAll()
            .Where(p => p.Score >= minScore)
            .OrderByDescending(p => p.Score)
            .ThenBy(p => p.Name)
            .ToList();

    Console.WriteLine($"{results.Count} player(s) with score >= {minScore}:");
    foreach (var p in results)
        Console.WriteLine($"   {p}");
}