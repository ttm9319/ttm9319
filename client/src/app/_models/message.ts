export interface Message{
  id: number;
  dateRead: Date;
   senderId: number;
   senderUsername: string;
   senderPhotoUrl: string;
   recipientId: number;
   recipientUsername: string;
   recipientPhotoUrl: string;
   content: string; 
   messageSent: Date;
   
}



