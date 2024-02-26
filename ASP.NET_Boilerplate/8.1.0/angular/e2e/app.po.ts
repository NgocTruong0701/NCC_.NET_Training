import { browser, element, by } from 'protractor';

export class NET_BoilerplateTemplatePage {
  navigateTo() {
    return browser.get('/');
  }

  getParagraphText() {
    return element(by.css('app-root h1')).getText();
  }
}
