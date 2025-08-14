from signalrcore.hub_connection_builder import HubConnectionBuilder
import logging
import json
from dataclasses import dataclass
from enum import Enum
from dataclasses import dataclass, asdict
from time import sleep

logging.basicConfig(level=logging.DEBUG)

# class CrossAreaFilter(Enum):
#     Include = "Include"
#     Exclude = "Exclude"

# @dataclass
class SignalRClientFilter:
    ParentMarketArea: str = None
    # Area: str = None
    # CrossAreaFilter: CrossAreaFilter = CrossAreaFilter.Include

@dataclass
class EpexTrade:
    trade_id: int
    contract_id: int
    quantity: float
    price: float
    trade_execution_time: str
    buy_delivery_area_id: str = None
    sell_delivery_area_id: str = None

server_url = "http://localhost:5000/trades"

# Get token from MSAL auth
# def get_token():
#     msal = MsalAuth(EpexTradesAuth.interactive_nonprod(use_persistent_token_cache=True))
#     token = msal.get_access_token_interactive()
#     return token

# def list_attributes(obj):
#     for attr in dir(obj):
#         if not attr.startswith("__"):
#             print(f"{attr}: {getattr(obj, attr)}")

# Build hub connection
hub_connection = HubConnectionBuilder()\
    .with_url(server_url)\
    .configure_logging(logging.DEBUG)\
    .build()

# Register handlers
hub_connection.on("Trade", lambda data: print("Trade:", data))
hub_connection.on("Subscribed", lambda data: print("Subscribed:", data))
hub_connection.on_open(lambda: print("Connected to the SignalR hub"))
hub_connection.on_close(lambda: print("Disconnected from the SignalR hub"))
hub_connection.on_error(lambda e: print("Error:", e))

# Start connection

# list_attributes(hub_connection)

# if not getattr(hub_connection, "is_connected", False):
hub_connection.start()
sleep(1)

# Create filter and subscribe
filter = SignalRClientFilter()
# filter = {
#     "ParentMarketArea": "AT",
#     "Area": "APG",
#     "CrossAreaFilter": "Include"
# }
#"AT" "APG" "Include"
filter.ParentMarketArea = "AT"
# filter.Area = "APG"
# filter.CrossAreaFilter = CrossAreaFilter.Include

hub_connection.send("Subscribe", [])

sleep(1)
print(f"Subscribed")

message = None
while message != "exit()":
    message = input(">> ")

hub_connection.stop()
print("Disconnected from the SignalR hub")