import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatIconRegistry } from '@angular/material/icon';
import { DomSanitizer } from '@angular/platform-browser';
import { BreweriesApiService, Brewery } from './ApiServices/breweries-api.service';
import { DialogBebeuMuitoComponent } from './components/dialog-bebeu-muito/dialog-bebeu-muito.component';

const BEER_ICON = `<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" aria-hidden="true" focusable="false" width="1em" height="1em" style="-ms-transform: rotate(360deg); -webkit-transform: rotate(360deg); transform: rotate(360deg);" preserveAspectRatio="xMidYMid meet" viewBox="0 0 24 24"><path d="M4 2h15l-2 20H6L4 2m2.2 2l1.6 16h1L7.43 6.34C8.5 6 9.89 5.89 11 7c1.56 1.56 4.33.69 5.5.23L16.8 4H6.2z" fill="#fff"/></svg>`;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'breweries';
  loading = false;
  panelOpenState = false;
  cervejarias: Array<Brewery> = [];
  contadorCervejas = 0;
  bebeuMuito: string = null;

  constructor(
    private breweriesApiService: BreweriesApiService,
    private dialog: MatDialog,
    iconRegistry: MatIconRegistry,
    sanitizer: DomSanitizer
  ) {
    iconRegistry.addSvgIconLiteral('beer', sanitizer.bypassSecurityTrustHtml(BEER_ICON));
  }

  ngOnInit(): void {
    this.listarCervejarias(false);
  }

  listarCervejarias(cached: boolean): void {
    this.loading = true;
    this.breweriesApiService.getBreweries(cached).subscribe(cervejariasApi => {
      this.cervejarias = cervejariasApi;
      this.loading = false;
    });
  }

  pedirCerveja(idCervejaria: number): void {
    this.loading = true;
    this.breweriesApiService.getBeers().subscribe(cerveja => {
      if (this.bebeuMuito) {
        this.bebeuMuito = null;
      }
      this.contadorCervejas++;
      this.loading = false;
    }, erro => {
      this.bebeuMuito = erro.error;
      this.negarCerveja();
      this.loading = false;
    });
  }

  negarCerveja(): void {
    const dialogRef = this.dialog.open(DialogBebeuMuitoComponent, {
      width: '400px',
      panelClass: 'custom-dialog-container',
      data: { mensagem: this.bebeuMuito }
    });
  }
}
