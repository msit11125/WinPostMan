using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinPostMan
{
    public interface IHttpService
    {
        Task<string> tryHttpGet(IDictionary<string, string> headers, string url, CancellationToken ct);
        Task<string> tryHttpPost(IDictionary<string, string> headers, string url, string jsonStr, CancellationToken ct);
    }

    public class HttpService : IHttpService
    {
        async public Task<string> tryHttpGet(IDictionary<string, string> headers, string url, CancellationToken ct)
        {
            using (var client = new HttpClient())
            {

                //讀取Url
                string stringContent;
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
                    //製作Headers
                    client.DefaultRequestHeaders.Accept.Add(
                             new MediaTypeWithQualityHeaderValue(headers.SingleOrDefault(h => h.Key == "Content-Type").Value));
                    headers.Remove("Content-Type");
                    foreach (var head in headers) //剩下的Header
                    {
                        request.Headers.Add(head.Key, head.Value);
                    };

                    var respon = await client.SendAsync(request, ct);
                    stringContent = await respon.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    stringContent = ex.ToString();
                }
                return stringContent;
            }
        }
        async public Task<string> tryHttpPost(IDictionary<string, string> headers, string url, string jsonStr, CancellationToken ct)
        {
            using (var client = new HttpClient())
            {
                //讀取Url
                string stringContent;
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
                    //製作Headers
                    client.DefaultRequestHeaders.Accept.Add(
                             new MediaTypeWithQualityHeaderValue(headers.SingleOrDefault(h => h.Key == "Content-Type").Value));
                    headers.Remove("Content-Type");
                    foreach (var head in headers) //剩下的Header
                    {
                        request.Headers.Add(head.Key, head.Value);
                    };
                    //製作Body
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json"); //CONTENT-TYPE header
                    request.Content = content;

                    var respon = await client.SendAsync(request, ct);
                    stringContent = await respon.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    stringContent = ex.ToString();
                }
                return stringContent;
            }
        }

    }
}
