﻿using GodSharp.Sockets.Internal.Extension;
using System;
using System.Net;
using System.Net.Sockets;

namespace GodSharp.Sockets
{
    /// <summary>
    /// Socket client
    /// </summary>
    public partial class SocketClient:SocketBase
    {
        private Listener listener = null;

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        public Sender Sender => listener?.Sender;

        /// <summary>
        /// Gets the remote end point.
        /// </summary>
        /// <value>
        /// The remote end point.
        /// </value>
        public EndPoint RemoteEndPoint => socket.RemoteEndPoint;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public SocketClient(string host,int port, ProtocolType protocolType= ProtocolType.Tcp)
        {
            SetHost(host);

            SetPort(port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocolType);
        }

        /// <summary>
        /// Connects this instance.
        /// </summary>
        public void Connect()
        {
            if (Connected)
            {
                return;
            }

            socket.Connect(Host, Port);

            SetOnConnectedFun();
        }

        /// <summary>
        /// Connects the specified end point.
        /// </summary>
        /// <param name="endPoint">The end point.</param>
        /// <exception cref="ArgumentNullException">endPoint</exception>
        public void Connect(EndPoint endPoint)
        {
            if (Connected)
            {
                return;
            }

            if (endPoint == null)
            {
                throw new ArgumentNullException(nameof(endPoint));
            }

            this.Host = endPoint.GetHost();
            this.Port = endPoint.GetPort();

            socket.Connect(endPoint);

            SetOnConnectedFun();
        }

        /// <summary>
        /// Connects the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="port">The port.</param>
        public void Connect(string host, int port)
        {
            if (Connected)
            {
                return;
            }

            SetHost(host);

            SetPort(port);

            socket.Connect(Host, Port);

            SetOnConnectedFun();
        }

        /// <summary>
        /// Connects the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="port">The port.</param>
        /// <exception cref="ArgumentNullException">address</exception>
        public void Connect(IPAddress address, int port)
        {
            if (Connected)
            {
                return;
            }

            if (address==null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            Host = address.ToString();
            socket.Connect(address, Port);

            SetOnConnectedFun();
        }
        
        /// <summary>
        /// Start Socket client.
        /// </summary>
        public override void Start()
        {
            try
            {
                if (listener?.Running==true)
                {
                    return;
                }

                if (listener==null)
                {
                    listener = new Listener(this,socket);
                }

                listener.Start();
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void Stop()
        {
            if (listener?.Running == true)
            {
                listener.Stop();
            }
        }
        
        /// <summary>
        /// Sets the on connected fun.
        /// </summary>
        private void SetOnConnectedFun()
        {
            if (Connected)
            {
                OnConnected?.Invoke(listener.Sender);
            }
        }
    }
}