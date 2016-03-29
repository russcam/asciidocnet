namespace AsciidocNet
{
	public interface IParser
	{
		Document Process(IDocumentReader reader);
	}
}