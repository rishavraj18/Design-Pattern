﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealisticDependencies;

namespace BehavioralPatterns.Mediator {
    public class FoodCartMediator : IMediator {
        private readonly IApplicationLogger _logger;

        private readonly Dictionary<string, ICommunicates> _fleet = new();

        /// <summary>
        /// We could initialize the mediator with a collection of ICommunicators
        /// </summary>
        public FoodCartMediator(IApplicationLogger logger) {
            _logger = logger;
        }

        public async Task Broadcast(NetworkMessage message) {
            _logger.LogInfo("[Mediator] ---> Broadcasting");
            foreach (var member in _fleet) {
                await member.Value.Receive(message);
            }
        }

        public async Task DeliverPayload(string handle, NetworkMessage message) {
            _logger.LogInfo($"[Mediator] ---> Delivering Payload to {handle}", ConsoleColor.DarkGray);
            if (!_fleet.ContainsKey(handle)) {
                _logger.LogError($"No handle: {handle}");
                return;
            }
            await _fleet[handle].Receive(message);
        }

        public async Task DeliverPayload(List<FleetMember> receivers, NetworkMessage message) {
            _logger.LogInfo($"[Mediator] ---> Delivering Payload to multiple", ConsoleColor.DarkGray);
            foreach (var member in receivers.Where(member => _fleet.ContainsKey(member.Handle))) {
                await member.Receive(message);
            }
        }

        public async Task Register(ICommunicates fleetMember) {
            await Task.Delay(250);
            if (!_fleet.ContainsKey(fleetMember.Handle)) {
                _fleet[fleetMember.Handle] = fleetMember;
            }
            fleetMember.SetMediator(this);
        }
    }
}
