namespace AsciiDocNet
{
	public interface IParser
	{
		Document Process(IDocumentReader reader);
	}
}