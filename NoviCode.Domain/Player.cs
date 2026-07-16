namespace NoviCode;

public class Player
{
	public Guid Id { get; }
	public string Name { get; }
	public int Score { get; private set; }

	private Player(Guid id, string name, int score)
	{
		Id = id;
		Name = name;
		Score = score;
	}

	public static Player CreateNew(Guid id, string name, int score)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new ArgumentException("Name cannot be null or empty.", nameof(name));

		return new Player(id, name, score);
	}

	public void UpdateScore(int newScore)
	{
		if (newScore < 0)
			throw new ArgumentOutOfRangeException(nameof(newScore), "Score cannot be negative.");

		Score = newScore;
	}

	public override string ToString() =>
		$"[{Id}] {Name} - Score: {Score}";
}
