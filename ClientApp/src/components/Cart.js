import React, { Component } from 'react';
import Modal from "react-bootstrap/Modal";
import "./css/Cart.css";
import authService from './api-authorization/AuthorizeService'
import modal from "bootstrap/js/src/modal";
import {forEach} from "react-bootstrap/ElementChildren";

export class Cart extends Component {
    constructor(props) {
        super(props);
        this.state = {pizzas: [], loading: true, cost: 0, shown: false};
    }
    
    componentDidMount() {
        this.populateCart();
    }
    
    async populateCart() {
        const token = await authService.getAccessToken();
        const response = await fetch('api/Pizza/cart', {
            headers: !token ? {} : {Authorization: `Bearer ${token}`}
        });
        let c = 0
        const data = await response.json();
        for (let i = 0; i < data.length; i++) {
            c += data[i].pizza.price;
        }
        this.setState({pizzas: data, loading: false, cost: c});
        console.log(this.state.pizzas);
    }
    
    async RemoveOrder(id) {
        const token = await authService.getAccessToken();
        await fetch('api/Pizza/cart/' + id, {
            method: 'DELETE',
            headers: !token ? {} : {Authorization: `Bearer ${token}`}
        });
        await this.populateCart();
    }
    
    async Order() {
        const token = await authService.getAccessToken();
        await fetch('api/Pizza/cart', {
            method: 'PUT',
            headers: !token ? {} : {Authorization: `Bearer ${token}`}
        });
        await this.populateCart();
    }
    
    async ClearCart() {
        const token = await authService.getAccessToken();
        await fetch('api/Pizza/cart/deleteall', {
            method: 'DELETE',
            headers: !token ? {} : {Authorization: `Bearer ${token}`}
        });
        await this.populateCart();
    }
        
    
    showmodal() {
        this.setState({shown: true});
        this.populateCart();
    }
    
    hidemodal() {
        this.setState({shown: false});
    }
    
    
    renderCart(pizzas) {
        return (
            <div className={"cart"}>
                <button className={"btn btn-primary"} onClick={() => this.showmodal()}>Cart</button>
                
                <Modal show={this.state.shown} onHide={() => this.hidemodal()} size={"lg"}>
                    <Modal.Header closeButton> <Modal.Title>Cart</Modal.Title> </Modal.Header>
                    <Modal.Body>
                        <table className='table table-striped' aria-labelledby="tabelLabel">
                            <thead>
                            <tr>
                                <th>Pizza image</th>
                                <th>Name</th>
                                <th>Price</th>
                                <th>Delete</th>
                            </tr>
                            </thead>
                            <tbody>
                            {pizzas.map(pizza => <tr style={{alignContent: "center"}} key={pizza.orderId}>
                                <td><img style={{width: "100px", height: "100px"}} src={pizza.pizza.imageUrl}
                                         alt={pizza.pizza.name}/></td>
                                <td className={"pizza-name"}>{pizza.pizza.name}</td>
                                <td>{pizza.pizza.price} PLN</td>
                                <td>
                                    <button className={"btn btn-danger"} onClick={async () => {
                                        await this.RemoveOrder(pizza.orderId)
                                    }}>Delete
                                    </button>
                                </td>
                            </tr>)}
                            </tbody>
                        </table>
                    </Modal.Body>
                    <Modal.Footer>
                        {this.state.cost > 0
                            ? <div className={"footer"}>
                                <p>Total cost: {this.state.cost} PLN</p>
                                <button className={"btn btn-success"} onClick={async () => {
                                    await this.Order()
                                }}>Order
                                </button>
                                <button className={"btn btn-danger"} onClick={async () => { await this.ClearCart() }}>Clear Cart </button>
                            </div>
                            : <p>So empty! Order some pizza here <a href={"/menu"}>Menu</a></p>}
                        <button className={"btn btn-secondary"} onClick={() => this.hidemodal()}>Close</button>
                    </Modal.Footer>
                </Modal>
                                                
            </div>
        );
    }
    
    
    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderCart(this.state.pizzas);
        
        return (
            <div>
                {contents}
            </div>
        );
    }
        
}