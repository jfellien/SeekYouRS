namespace SeekYouRS.Store
{
	public interface IQueryReadModels
	{
		T Retrieve<T>(dynamic query);
	}
}