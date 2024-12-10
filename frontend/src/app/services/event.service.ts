import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { Event } from '../models/event.model';
import { Response } from '../models/response.model';

@Injectable({
  providedIn: 'root',
})
export class EventService {
  private apiUrl = 'http://localhost:5140/api/EventLogs';

  constructor(private http: HttpClient) {}

  getEvents(): Observable<Event[]> {
    return this.http
      .get<Response<Event>>(`${this.apiUrl}?pageNumber=1&pageSize=10`)
      .pipe(map((response) => response.result));
  }
}
