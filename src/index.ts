import "./css/main.css";
import * as signalR from '@microsoft/signalr';

const divMessage: HTMLDivElement = document.querySelector("#divMessages");
const tbMessage: HTMLInputElement = document.querySelector("#tbMessage");
const btnSend: HTMLButtonElement = document.querySelector("#btnSend");

const username = new Date().getTime();

const connection =
    new signalR.HubConnectionBuilder()
    .withUrl("/hub")
    .build();

connection.on("messageReceived", (username: string, message: string) => {
    let m = document.createElement("div");

    m.innerHTML = `<div class="message-author"> ${username} </div><div>${message}</div>`;
    divMessage.appendChild(m);
    divMessage.scrollTop = divMessage.scrollHeight;
});

connection.start().catch(err => document.write(err));

tbMessage.addEventListener("keyup", (e: KeyboardEvent) => {
    if (e.key === "Enter") {
        send();
    }
})

btnSend.addEventListener("click", send);

function send() {
    connection.send("newMessage", username, tbMessage.value)
        .then(() => tbMessage.value = "");
}