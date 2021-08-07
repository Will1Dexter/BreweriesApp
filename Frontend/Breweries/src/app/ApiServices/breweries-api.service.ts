import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface Brewery {
  id: number;
  name: string;
  city: string;
  state: string;
  country: string;
}

export interface Beer {
  mensagem: string;
}

@Injectable()
export class BreweriesApiService {

  private breweriesUrl = 'http://localhost:1095';

  constructor(private http: HttpClient) { }

  public getBreweries(cached: boolean): Observable<Array<Brewery>> {
    return this.http.get<Array<Brewery>>(`${this.breweriesUrl}/breweries?cached=${cached ? 'true' : 'false'}`);
  }

  public getBeers(): Observable<Beer> {
    return this.http.get<Beer>(`${this.breweriesUrl}/beers`);
  }
}
