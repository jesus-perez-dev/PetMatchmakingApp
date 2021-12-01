export class Message {
  id: number;
  senderId: number;
  senderUsername: string;
  senderProfilePicture: string;
  receiverId: number;
  receiverUsername: string;
  receiverProfilePicture: string;
  content: string;
  dateSent: Date;
  dateRead?: Date;
}
