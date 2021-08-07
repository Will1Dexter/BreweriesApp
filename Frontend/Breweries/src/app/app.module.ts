import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { HttpClientModule } from '@angular/common/http';
import { BreweriesApiService } from './ApiServices/breweries-api.service';
import { DialogBebeuMuitoComponent } from './components/dialog-bebeu-muito/dialog-bebeu-muito.component';

@NgModule({
  declarations: [
    AppComponent,
    DialogBebeuMuitoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatProgressBarModule,
    MatCardModule,
    MatButtonModule,
    MatExpansionModule,
    MatDividerModule,
    MatIconModule,
    MatDialogModule,
    BrowserAnimationsModule,
    HttpClientModule
  ],
  providers: [BreweriesApiService],
  bootstrap: [AppComponent]
})
export class AppModule { }
