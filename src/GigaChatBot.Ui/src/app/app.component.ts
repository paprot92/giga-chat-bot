import {
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  effect,
  ElementRef,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { ConversationClientService } from './services/conversation-client.service';
import { CommonModule } from '@angular/common';
import { IConversation, MessageType } from './models/conversation.model';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { first, tap } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [CommonModule, ReactiveFormsModule],
  providers: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent implements OnInit {
  private _cd = inject(ChangeDetectorRef);
  private _conversationClient = inject(ConversationClientService);
  @ViewChild('conversationContainer')
  private conversationContainer!: ElementRef;
  protected MessageType = MessageType;
  protected conversationDetails = signal<IConversation | null>(null);
  protected currentQuestion = signal<string | null>(null);
  protected currentResponse = this._conversationClient.currentResponse;
  protected inputFormControl = new FormControl<string | undefined>(undefined);

  constructor() {
    effect(() => {
      this.conversationDetails();
      this.currentQuestion();
      this.currentResponse();
      this.scrollToBottom();
    });

    effect(() => {
      if (!!this.currentQuestion()?.length && !this.inputFormControl.disabled) {
        this.inputFormControl.disable();
      } else if (this.inputFormControl.disabled) {
        this.inputFormControl.enable();
      }
    });
  }

  ngOnInit(): void {
    this.reloadConversation();
  }

  protected onKeyPress(conversationId: string, event: KeyboardEvent): void {
    if (event.key === 'Enter' && this.inputFormControl.value) {
      this.currentQuestion.set(this.inputFormControl.value);
      this._conversationClient
        .sendMessage(conversationId, this.inputFormControl.value)
        .pipe(
          tap(() => {
            this.reloadConversation();
          })
        )
        .subscribe();
      this.inputFormControl.reset();
    }
  }

  private reloadConversation(): void {
    this._conversationClient
      .getTestConversationDetails()
      .pipe(
        first(),
        tap((conversation) => this.conversationDetails.set(conversation)),
        tap(() => {
          this.currentQuestion.set(null);
          this.currentResponse.set(null);
          this.scrollToBottom();
        })
      )
      .subscribe();
  }

  private scrollToBottom() {
    setTimeout(() => {
      if (this.conversationContainer?.nativeElement != null) {
        this.conversationContainer.nativeElement.scrollTop =
          this.conversationContainer.nativeElement.scrollHeight;
      }
    }, 200);
  }
}
