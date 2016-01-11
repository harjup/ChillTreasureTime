public class Milestone 
{
    public int Amount { get; private set; }

    public string Id { get; private set; }

    public Milestone(int amount, string id)
    {
        Amount = amount;
        Id = id;
    }

}
