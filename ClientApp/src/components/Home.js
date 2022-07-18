import React, {Component} from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
          <h1>Hello to a pizzeria website built with React! Backend in ASP.NET Core</h1>
          <p>This is my first learning project, it was used to learn both ASP.NET Core MCV + React, and I think it really helped to unserstand the frameworks.</p>
      </div>
    );
  }
}
