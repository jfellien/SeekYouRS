namespace SeekYouRS.Tests
{
    public class UI
    {
        private readonly KundenApi _kundenApi;

        public UI()
        {
            _kundenApi = new KundenApi(new KundenCommandReciever(new InMemoryEventStore()), new KundenQueries());
        }

        public void ErfasseKunde(string name)
        {
            _kundenApi.Commands.Recieve(new ErfasseKunde{Name = name});
        }
    }
}