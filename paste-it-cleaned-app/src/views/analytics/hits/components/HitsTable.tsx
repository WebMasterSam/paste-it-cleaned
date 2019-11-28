import React, { Fragment } from 'react'

import { Table, TableHead, TableRow, TableBody, TableCell, TablePagination } from '@material-ui/core'

export interface HitsTableProps {
    rows: any[]
}
export interface HitsTableState {
    rows: any[]
    page: number
    rowsPerPage: number
}

const columns = [
    { id: 'timeStamp', label: 'Date/time', minWidth: 150 },
    { id: 'type', label: 'Type', minWidth: 100 },
    { id: 'ip', label: 'Client IP', minWidth: 75 },
    { id: 'userAgent', label: 'User agent', minWidth: 150 },
    { id: 'price', label: 'Price (USD)', align: 'right', minWidth: 75 },
]

class HitsTable extends React.Component<HitsTableProps, HitsTableState> {
    constructor(props: HitsTableProps) {
        super(props)
        this.state = { page: 0, rowsPerPage: 10, rows: props.rows }
        this.handleChangePage = this.handleChangePage.bind(this)
        this.handleChangeRowsPerPage = this.handleChangeRowsPerPage.bind(this)
    }

    handleChangePage(event: any, newPage: any) {
        this.setState({ page: newPage })
    }

    handleChangeRowsPerPage(event: any) {
        this.setState({ rowsPerPage: event.target.value })
        this.setState({ page: 0 })
    }

    render() {
        return (
            <Fragment>
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
                            {this.state.rows.slice(this.state.page * this.state.rowsPerPage, this.state.page * this.state.rowsPerPage + this.state.rowsPerPage).map(row => {
                                return (
                                    <TableRow hover tabIndex={-1} key={row.key}>
                                        {columns.map(column => {
                                            const value = (row as any)[column.id] as any
                                            return (
                                                <TableCell key={column.id} align={column.align as any}>
                                                    {value}
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
                    count={this.state.rows.length}
                    rowsPerPage={this.state.rowsPerPage}
                    page={this.state.page}
                    backIconButtonProps={{
                        'aria-label': 'previous page',
                    }}
                    nextIconButtonProps={{
                        'aria-label': 'next page',
                    }}
                    onChangePage={this.handleChangePage}
                    onChangeRowsPerPage={this.handleChangeRowsPerPage}
                />
            </Fragment>
        )
    }
}

export default HitsTable
