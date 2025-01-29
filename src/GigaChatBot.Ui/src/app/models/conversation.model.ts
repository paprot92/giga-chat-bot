export interface IConversation {
  id: string;
  messages: IMessage[];
}

export interface IMessage {
  id: string;
  content: string;
  createdOn: Date;
  type: MessageType;
  reaction: MessageReaction;
}

export enum MessageType {
  System,
  User,
  Assistant,
}

export enum MessageReaction {
  None,
  Like,
  Dislike,
}
