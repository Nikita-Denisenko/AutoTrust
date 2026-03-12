namespace AutoTrust.Domain.ValueObjects
{
    public record Url
    {
        public string Value { get; }

        private Url(string value)
        {
            Value = value;
        }

        public static Url Create(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("Url cannot be empty");

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            {
                throw new ArgumentException($"Invalid URL format: {url}");
            }

            return new Url(url);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public override string ToString() => Value;
    }
}
