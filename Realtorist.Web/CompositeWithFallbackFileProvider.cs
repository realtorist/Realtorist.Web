using System;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace Realtorist.Web
{
    /// <summary>
    /// File provider which uses fallback file provider if main one has failed.
    /// Is used in order for WebOptimezer work properly
    /// </summary>
    public class CompositeWithFallbackFileProvider : IFileProvider
    {
        private readonly CompositeFileProvider _mainFileProvider;
        private readonly PhysicalFileProvider _fallbackFileProvider;

        public CompositeWithFallbackFileProvider(CompositeFileProvider mainFileProvider, PhysicalFileProvider fallbackFileProvider)
        {
            _mainFileProvider = mainFileProvider ?? throw new ArgumentNullException(nameof(mainFileProvider));
            _fallbackFileProvider = fallbackFileProvider ?? throw new ArgumentNullException(nameof(fallbackFileProvider));
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            var main = _mainFileProvider.GetDirectoryContents(subpath);
            return main.Exists ? main : _fallbackFileProvider.GetDirectoryContents(subpath);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            var main = _mainFileProvider.GetFileInfo(subpath);
            return main.Exists ? main : _fallbackFileProvider.GetFileInfo(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return _mainFileProvider.Watch(filter);
        }
    }
}