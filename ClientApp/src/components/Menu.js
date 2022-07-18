import React, { Component } from 'react';
import "./css/Menu.css";
import authService from './api-authorization/AuthorizeService'
import {Cart} from './Cart';

export class Menu extends Component {
    constructor(props) {
        super(props);
        this.state = {pizzas: [], loading: true};

    }

    componentDidMount() {
        this.populateMenu();
    }


    renderMenu(pizzas) {
        return (
            <div>
                <Cart/>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                    <tr>
                        <th>Pizza image</th>
                        <th>Name</th>
                        <th>Price</th>
                        <th>Description</th>
                        <th>Ingridients</th>
                        <th>Order</th>
                    </tr>
                    </thead>
                    <tbody>
                    {pizzas.map(pizza =>
                        <tr style={{alignContent: "center"}} key={pizza.id}>
                            <td><img style={{width: "200px", height: "200px"}} src={pizza.imageUrl} alt={pizza.name}/></td>
                            <td className={"pizza-name"}>{pizza.name}</td>
                            <td>{pizza.price} PLN</td>
                            <td>{pizza.description}</td>
                            <td>{pizza.ingredients}</td>
                            <td>
                                <button className={"btn btn-primary"} onClick={async () => {await this.orderPizza(pizza.id)}}>Order</button>
                            </td>
                        </tr>
                    )}
                    </tbody>
                </table>
            </div>
            
            
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderMenu(this.state.pizzas);

        return (
            <div>
                <h1 id="tabelLabel">Pizzeria menu</h1>
                <p>This is our menu!</p>
                {contents}
            </div>
        );
    }

    async populateMenu() {
        const d = await fetch('api/pizza');
        const data = await d.json();
        console.log(data);
        this.setState({pizzas: data, loading: false});
    }

    async orderPizza(id) {
        const token = await authService.getAccessToken();
        const response = await fetch('api/Pizza/'+id, {
            method: 'POST',
            headers: !token ? {} : { 'Authorization': `Bearer ${token}` },
            body: JSON.stringify({id: id})
        });
        const data = await response.json();
        console.log(data);
    }
} 
    
