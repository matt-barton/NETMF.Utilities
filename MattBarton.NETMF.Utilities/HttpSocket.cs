using System.Net;
using System.Net.Sockets;
using MattBarton.NETMF.Utilities.Adapters;
using MattBarton.NETMF.Utilities.Interfaces;
using MattBarton.NETMF.Utilities.Services;
using System;
using MattBarton.NETMF.Utilities.Exceptions.Http;
using System.Text;

namespace MattBarton.NETMF.Utilities
{
	public class HttpSocket : IHttpSocket
    {
        #region Fields

        private ISocket _socket;
        private ITimerService _timerService;
        private IDns _dns;

        public static int BufferSize = 1024;

        #endregion

        #region Properties

        private ISocket Socket
        {
            get
            {
                return this._socket ??
                    (this._socket = new SocketAdapter(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
            }
        }

        private ITimerService TimerService
        {
            get
            {
                return this._timerService ??
                    (this._timerService = new TimerService());
            }
        }

        private IDns Dns
        {
            get
            {
                return this._dns;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public HttpSocket()
        {
        }

        /// <summary>
        /// Dependency injection
        /// </summary>
        /// <param name="socket"></param>
        public HttpSocket(ISocket socket, ITimerService timerService, IDns dns)
        {
            this._socket = socket;
            this._timerService = timerService;
            this._dns = dns;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Connect to host over a given port
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void Connect (string host, int port)
		{

            IPHostEntry hostEntry;

            try
            {
                if (this.Dns != null)
                {
                    // this here for unit testing
                    hostEntry = this.Dns.GetHostEntry(host);
                }
                else
                {
                    hostEntry = System.Net.Dns.GetHostEntry(host);
                }
            }
            catch (SocketException e)
            {
                throw new HttpUnknownHostException("The host " + host + " could not be found.", e);
            }
            
            try
            {
                this.Socket.Connect(new IPEndPoint(hostEntry.AddressList[0], port));
            }
            catch (SocketException e)
            {
                throw new HttpSocketConnectionException("The socket threw a SocketException", e);
            }
        }

        /// <summary>
        /// Send a request to the connected host
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string Request(HttpRequest request)
        {
            this.Socket.Send(request.ToByteArray());

            while (this.Socket.Available() < 1)
            {
                if (this.TimerService.TimeoutReached() == false)
                {
                    this.TimerService.Sleep(100);
                }
                else
                {
                    throw new HttpTimeoutException("The host at " + request.Hostname + " did not respond in a timely manner.");
                }
            }

            var response = "";
            while (this.Socket.Available() > 0)
            {
                var bytes = this.Socket.Receive(HttpSocket.BufferSize);
                response += new String(Encoding.UTF8.GetChars(bytes));
            }

            return response;
        }

        #endregion
    }
}
