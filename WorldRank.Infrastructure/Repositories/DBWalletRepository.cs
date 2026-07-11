using Microsoft.Extensions.Logging;
using WorldRank.Application.Interfaces;
using WorldRank.Domain.Entities;
using WorldRank.Domain.Enums;
using WorldRank.Domain.Exceptions;

namespace WorldRank.Infrastructure.Repositories;

public class DBWalletRepository : IWalletRepository
{
    private readonly WorldRankDbContext.WorldRankDbContext _context;
    private readonly ILogger<DBWalletRepository> _logger;

    public DBWalletRepository(WorldRankDbContext.WorldRankDbContext context, ILogger<DBWalletRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public void Add(Wallet wallet)
    {
        var exists = _context.Wallets.Any(item => item.PlayerId == wallet.PlayerId && item.Currency == wallet.Currency);

        if (exists)
        {
            throw new DuplicateWalletException(wallet.PlayerId, wallet.Currency);
        }

        _context.Wallets.Add(wallet);
        _context.SaveChanges();
        _logger.LogInformation("Wallet created for player {PlayerId} in {Currency} with balance {Balance}", wallet.PlayerId, wallet.Currency, wallet.Balance);
    }

    public List<Wallet> GetAllWalletsByPlayerId(int playerId)
    {
        return _context.Wallets.Where(item => item.PlayerId == playerId).ToList();
    }

    public Wallet GetWallet(int playerId, Currency currency)
    {
        var wallet = _context.Wallets.SingleOrDefault(item => item.PlayerId == playerId && item.Currency == currency);

        if (wallet is null)
        {
            throw new WalletNotFoundException(playerId, currency);
        }

        return wallet;
    }

    public void UpdateBalance(int playerId, Currency currency, decimal newBalance)
    {
        var wallet = GetWallet(playerId, currency);
        wallet.SetBalance(newBalance);
        _context.SaveChanges();
        _logger.LogInformation("Player {PlayerId} {Currency} wallet balance set to {Balance}", playerId, currency, newBalance);
    }

    public void Deposit(int playerId, Currency currency, decimal amount)
    {
        var wallet = GetWallet(playerId, currency);
        wallet.Deposit(amount);
        _context.SaveChanges();
        _logger.LogInformation("Deposited {Amount} to player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
    }

    public void Withdraw(int playerId, Currency currency, decimal amount)
    {
        var wallet = GetWallet(playerId, currency);
        wallet.Withdraw(amount);
        _context.SaveChanges();
        _logger.LogInformation("Withdrew {Amount} from player {PlayerId} {Currency} wallet (balance {Balance})", amount, playerId, currency, wallet.Balance);
    }

    public void Block(int playerId, Currency currency)
    {
        var wallet = GetWallet(playerId, currency);
        wallet.Block();
        _context.SaveChanges();
        _logger.LogInformation("Player {PlayerId} {Currency} wallet blocked", playerId, currency);
    }

    public void Unblock(int playerId, Currency currency)
    {
        var wallet = GetWallet(playerId, currency);
        wallet.Unblock();
        _context.SaveChanges();
        _logger.LogInformation("Player {PlayerId} {Currency} wallet unblocked", playerId, currency);
    }
}