import React, { Component } from 'react';

export class FetchCars extends Component {
    static displayName = FetchCars.name;

    constructor(props) {
        super(props);
        this.state = { cars: [], loading: true };
    }

    componentDidMount() {
        this.populateCarData();
    }

    static renderCarsTable(cars) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Make</th>
                        <th>Model</th>
                        <th>Trim</th>
                        <th>Year</th>
                    </tr>
                </thead>
                <tbody>
                    {cars.map(car =>
                        <tr key={car.id}>
                            <td>{car.make}</td>
                            <td>{car.model}</td>
                            <td>{car.trim}</td>
                            <td>{car.year}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : FetchCars.renderCarsTable(this.state.cars);

        return (
            <div>
                <h1 id="tabelLabel" >All cars</h1>
                <p>This component demonstrates fetching data from the server.</p>
                {contents}
            </div>
        );
    }

    async populateCarData() {
        const response = await fetch('cars');
        const data = await response.json();
        this.setState({ cars: data, loading: false });
    }
}
