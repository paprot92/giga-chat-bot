import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { Observable, Subject, takeUntil } from 'rxjs';
import { IConversation, MessageReaction } from '../models/conversation.model';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class ConversationClientService {
  // todo: refactor; separate state service (optimize reloads) and api client service;
  private readonly _apiUrl = `https://localhost:7044`;
  private readonly _conversationEndpoint = `${this._apiUrl}/conversation`;
  private readonly _messageEndpoint = `${this._apiUrl}/message`;
  private hubConnection!: signalR.HubConnection;
  private cancelResponseGenerationSubject = new Subject<void>();

  currentResponse = signal<string | null>(null);

  constructor(private http: HttpClient) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this._conversationEndpoint}/hub`)
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => console.log('SignalR Connected'))
      .catch((err) => console.error('SignalR Connection Error: ', err));

    this.hubConnection.on(
      'ReceiveMessage',
      (conversationId: string, message: string) => {
        if (this.currentResponse() == null) {
          this.currentResponse.set(message);
        } else {
          this.currentResponse.update((v) => `${v} ${message}`);
        }
      }
    );
  }

  getTestConversationDetails(): Observable<IConversation> {
    return this.http.get<IConversation>(`${this._conversationEndpoint}/test`);
  }

  sendMessage(conversationId: string, message: string): Observable<void> {
    this.cancelResponseGenerationSubject = new Subject<void>();
    return this.http
      .post<void>(`${this._conversationEndpoint}/${conversationId}/message`, {
        conversationId: conversationId,
        content: message,
      })
      .pipe(takeUntil(this.cancelResponseGenerationSubject));
  }

  cancelResponseGeneration(): void {
    this.cancelResponseGenerationSubject.next();
    this.cancelResponseGenerationSubject.complete();
  }

  reactToMessage(
    messageId: string,
    reaction: MessageReaction
  ): Observable<void> {
    return this.http.post<void>(`${this._messageEndpoint}/${messageId}/react`, {
      messageId,
      reaction,
    });
  }
}
