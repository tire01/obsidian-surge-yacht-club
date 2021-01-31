using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LanguageExt;
using static LanguageExt.Prelude;

namespace RowerMoniter.Moniter
{
    public class SerialMoniter : IDisposable
    {
        private bool disposedValue;
        private readonly TextReader _reader;

        public SerialMoniter(TextReader reader )
        {
            _reader = reader;
        }

        public IEnumerable<object> Parse() 
        {
            var objects = new List<object>();


        }

        private Option<object> ParseLine(string line) 
        {
            var firstColonIndex = line.IndexOf(':');
            if (firstColonIndex < 0) 
            {
                return Option<object>.None;
            }

            var messageType = line.Substring(0, firstColonIndex)
            switch (messageType) 
            {
                
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _reader.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
