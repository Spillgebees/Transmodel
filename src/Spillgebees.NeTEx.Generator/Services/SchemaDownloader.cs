using System.Formats.Tar;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;
using System.Text.RegularExpressions;
using Spillgebees.NeTEx.Generator.Configuration;

namespace Spillgebees.NeTEx.Generator.Services;

public static partial class SchemaDownloader
{
    public static async Task<string> DownloadAndExtractAsync(
        string versionOrRef,
        bool isTag,
        Action<string>? log = null,
        CancellationToken cancellationToken = default)
    {
        var url = isTag
            ? string.Format(CultureInfo.InvariantCulture, GeneratorDefaults.GitHubArchiveUrlTemplate, versionOrRef)
            : string.Format(CultureInfo.InvariantCulture, GeneratorDefaults.GitHubArchiveRefUrlTemplate, versionOrRef);

        log?.Invoke($"Downloading schemas from {url}...");

        var tempDir = Path.Combine(Path.GetTempPath(), "netex-schemas", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Spillgebees.NeTEx.Generator/1.0");

        using var response = await httpClient.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();

        var archivePath = Path.Combine(tempDir, "netex.tar.gz");
        await using (var fileStream = File.Create(archivePath))
        {
            await response.Content.CopyToAsync(fileStream, cancellationToken);
        }

        log?.Invoke("Extracting schemas...");

        var extractDir = Path.Combine(tempDir, "extracted");
        Directory.CreateDirectory(extractDir);

        await using (var gzipStream = new GZipStream(File.OpenRead(archivePath), CompressionMode.Decompress))
        {
            await TarFile.ExtractToDirectoryAsync(gzipStream, extractDir, overwriteFiles: true, cancellationToken);
        }

        // GitHub archives extract to a folder like NeTEx-{version}/
        var innerDir = Directory.GetDirectories(extractDir).FirstOrDefault()
            ?? throw new InvalidOperationException("Failed to find extracted schema directory.");

        var xsdDir = Path.Combine(innerDir, "xsd");
        if (!Directory.Exists(xsdDir))
        {
            throw new InvalidOperationException($"XSD directory not found at {xsdDir}");
        }

        log?.Invoke($"Schemas extracted to {xsdDir}");
        return xsdDir;
    }

    public static async Task<IReadOnlyList<string>> ListVersionsAsync(
        CancellationToken cancellationToken = default)
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Spillgebees.NeTEx.Generator/1.0");
        httpClient.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");

        var versions = new List<string>();
        var url = GeneratorDefaults.GitHubApiTagsUrl + "?per_page=100";

        while (url is not null)
        {
            using var response = await httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(cancellationToken);
            var tags = JsonDocument.Parse(json);

            foreach (var tag in tags.RootElement.EnumerateArray())
            {
                if (tag.TryGetProperty("name", out var name))
                {
                    versions.Add(name.GetString()!);
                }
            }

            // Check for pagination via Link header
            url = null;
            if (response.Headers.TryGetValues("Link", out var linkHeaders))
            {
                var linkHeader = string.Join(",", linkHeaders);
                var nextMatch = NextLinkPattern().Match(linkHeader);
                if (nextMatch.Success)
                {
                    url = nextMatch.Groups[1].Value;
                }
            }
        }

        return versions;
    }

    [GeneratedRegex(@"<([^>]+)>;\s*rel=""next""")]
    private static partial Regex NextLinkPattern();
}
