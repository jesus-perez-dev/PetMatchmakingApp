import {Photo} from './photo';

export interface Member {
  id: number;
  userName: string;
  gender: string;
  bio: string;
  age: number;
  alias: string;
  accountRegistrationDate: Date;
  lastLogon: Date;
  seeking: string;
  interest: string;
  city: string;
  country: string;
  photos: Photo[];
  profilePhoto: string;
}
