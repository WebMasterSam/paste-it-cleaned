import React from 'react'
import { TextField, Paper, Grid, Button, Typography, Select, MenuItem, InputLabel, FormControl, Table, TableHead, TableRow, TableBody, TableCell, TablePagination } from '@material-ui/core'
import FormWrapper from '../../../components/FormWrapper'
import CreditCardIcon from '@material-ui/icons/CreditCard'
import HistoryIcon from '@material-ui/icons/History'

import './BillingInformation.less'
import { VisaIcon } from './components/Visa'
import { MasterCardIcon } from './components/MasterCard'
import { AmexIcon } from './components/Amex'

export interface BillingInformationProps {}

const columns = [
    { id: 'name', label: 'Name', minWidth: 170 },
    { id: 'code', label: 'ISO\u00a0Code', minWidth: 100 },
    {
        id: 'population',
        label: 'Population',
        minWidth: 170,
        align: 'right',
        format: (value: any) => value.toLocaleString(),
    },
    {
        id: 'size',
        label: 'Size\u00a0(km\u00b2)',
        minWidth: 170,
        align: 'right',
        format: (value: any) => value.toLocaleString(),
    },
    {
        id: 'density',
        label: 'Density',
        minWidth: 170,
        align: 'right',
        format: (value: any) => value.toFixed(2),
    },
]

function createData(name: any, code: any, population: any, size: any) {
    const density = population / size
    return { name, code, population, size, density }
}

const rows = [
    createData('India', 'IN', 1324171354, 3287263),
    createData('China', 'CN', 1403500365, 9596961),
    createData('Italy', 'IT', 60483973, 301340),
    createData('United States', 'US', 327167434, 9833520),
    createData('Canada', 'CA', 37602103, 9984670),
    createData('Australia', 'AU', 25475400, 7692024),
    createData('Germany', 'DE', 83019200, 357578),
    createData('Ireland', 'IE', 4857000, 70273),
    createData('Mexico', 'MX', 126577691, 1972550),
    createData('Japan', 'JP', 126317000, 377973),
    createData('France', 'FR', 67022000, 640679),
    createData('United Kingdom', 'GB', 67545757, 242495),
    createData('Russia', 'RU', 146793744, 17098246),
    createData('Nigeria', 'NG', 200962417, 923768),
    createData('Brazil', 'BR', 210147125, 8515767),
]

function BillingInformation() {
    const [page, setPage] = React.useState(0)
    const [rowsPerPage, setRowsPerPage] = React.useState(10)

    const handleChangePage = (event: any, newPage: any) => {
        setPage(newPage)
    }

    const handleChangeRowsPerPage = (event: any) => {
        setRowsPerPage(+event.target.value)
        setPage(0)
    }

    return (
        <React.Fragment>
            <Grid container spacing={3}>
                <Grid item xs={12} lg={4}>
                    <Paper className="paper wide">
                        <Typography variant="h2" className="override-h2" component="h2">
                            <CreditCardIcon /> Credit card
                        </Typography>

                        <FormWrapper>
                            <Grid container spacing={3}>
                                <Grid item xs={8}>
                                    <TextField name="owner" label="Name on card" fullWidth />
                                </Grid>
                                <Grid item xs={4}>
                                    <TextField name="cvv" label="CVV" fullWidth />
                                </Grid>
                            </Grid>

                            <Grid container spacing={3}>
                                <Grid item xs={12}>
                                    <TextField name="card-number" label="Card number" fullWidth />
                                </Grid>
                            </Grid>

                            <Grid container spacing={3}>
                                <Grid item xs={3}>
                                    <FormControl fullWidth>
                                        <InputLabel id="expiration-month-label">Month</InputLabel>
                                        <Select labelId="expiration-month-label" id="expiration-month" name="expiration-month" fullWidth>
                                            <MenuItem value={'jan'}>January</MenuItem>
                                        </Select>
                                    </FormControl>
                                </Grid>
                                <Grid item xs={2}>
                                    <FormControl fullWidth>
                                        <InputLabel id="expiration-year-label">Year</InputLabel>
                                        <Select labelId="expiration-year-label" id="expiration-year" name="expiration-year" fullWidth>
                                            <MenuItem value={'2019'}>2019</MenuItem>
                                        </Select>
                                    </FormControl>
                                </Grid>
                                <Grid item xs={7} className="align-right">
                                    <VisaIcon height={55} style={{ marginLeft: '10px' }} />
                                    <MasterCardIcon height={55} style={{ marginLeft: '10px' }} />
                                    <AmexIcon height={55} style={{ marginLeft: '10px' }} />
                                </Grid>
                            </Grid>
                        </FormWrapper>

                        <Grid container spacing={3}>
                            <Grid item xs={4}>
                                <Button variant="contained" type="submit" color="primary" disabled={false}>
                                    Update
                                </Button>
                            </Grid>
                        </Grid>
                    </Paper>
                </Grid>
                <Grid item xs={12} lg={8}>
                    <Paper className="paper wide">
                        <Typography variant="h2" className="override-h2" component="h2">
                            <HistoryIcon /> Billing history
                        </Typography>

                        <FormWrapper>
                            <div className="table-wrapper">
                                <Table stickyHeader aria-label="sticky table">
                                    <TableHead>
                                        <TableRow>
                                            {columns.map(column => (
                                                <TableCell key={column.id} align={column.align as any} style={{ minWidth: column.minWidth }}>
                                                    {column.label}
                                                </TableCell>
                                            ))}
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        {rows.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage).map(row => {
                                            return (
                                                <TableRow hover role="checkbox" tabIndex={-1} key={row.code}>
                                                    {columns.map(column => {
                                                        const value = (row as any)[column.id] as any
                                                        return (
                                                            <TableCell key={column.id} align={column.align as any}>
                                                                {column.format && typeof value === 'number' ? column.format(value) : value}
                                                            </TableCell>
                                                        )
                                                    })}
                                                </TableRow>
                                            )
                                        })}
                                    </TableBody>
                                </Table>
                            </div>
                            <TablePagination
                                rowsPerPageOptions={[10, 25, 100]}
                                component="div"
                                count={rows.length}
                                rowsPerPage={rowsPerPage}
                                page={page}
                                backIconButtonProps={{
                                    'aria-label': 'previous page',
                                }}
                                nextIconButtonProps={{
                                    'aria-label': 'next page',
                                }}
                                onChangePage={handleChangePage}
                                onChangeRowsPerPage={handleChangeRowsPerPage}
                            />
                        </FormWrapper>
                    </Paper>
                </Grid>
            </Grid>
        </React.Fragment>
    )
}

export default BillingInformation
