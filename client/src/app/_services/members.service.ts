import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';


@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor(private htttp: HttpClient) { }

  getMembers(){
    return this.htttp.get<Member[]>(this.baseUrl + 'users');
  }

  getMember(username: string){
    return this.htttp.get<Member>(this.baseUrl + 'users/' + username);
  }
}
