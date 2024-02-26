import { NET_BoilerplateTemplatePage } from './app.po';

describe('NET_Boilerplate App', function() {
  let page: NET_BoilerplateTemplatePage;

  beforeEach(() => {
    page = new NET_BoilerplateTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
