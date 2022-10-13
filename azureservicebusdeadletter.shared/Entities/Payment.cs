namespace azureservicebusdeadletter.shared.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }

        public Payment() 
        {
            this.Id = Guid.NewGuid();
        }
    }
}