import React, { Fragment } from 'react'

import { Table, TableHead, TableRow, TableBody, TableCell, TablePagination } from '@material-ui/core'
import { Skeleton } from '@material-ui/lab'
import LoadingError from '../../../../components/LoadingError'

export interface HitsTableProps {
    rows: any[]
    full: boolean
    loading: boolean
    error: boolean
}

export interface HitsTableState {
    rows: any[]
    page: number
    rowsPerPage: number
}

const columnsFull = [
    { id: 'number', label: 'Number', minWidth: 150 },
    { id: 'amount', label: 'Amount', minWidth: 150 },
    { id: 'status', label: 'Status', minWidth: 150 },
    { id: 'paidon', label: 'Paid on', align: 'right', minWidth: 200 },
    { id: 'pdf', label: '', align: 'center', minWidth: 30 },
]

const columnsCompact = [
    { id: 'number', label: 'Number', minWidth: 150 },
    { id: 'amount', label: 'Amount', minWidth: 150 },
    { id: 'status', label: 'Status', minWidth: 150 },
    { id: 'paidon', label: 'Paid on', align: 'right', minWidth: 200 },
    { id: 'pdf', label: '', align: 'center', minWidth: 30 },
]

class BillingTable extends React.Component<HitsTableProps, HitsTableState> {
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

    componentWillReceiveProps() {
        if (this.props.rows != this.state.rows) {
            this.setState({ rows: this.props.rows })
        }
    }

    componentDidUpdate() {
        if (this.props.rows != this.state.rows) {
            this.setState({ rows: this.props.rows })
        }
    }

    render() {
        if (this.props.loading) {
            return (
                <Fragment>
                    <Skeleton variant="text" height={75} />
                    <Skeleton variant="text" height={40} />
                    <Skeleton variant="text" height={40} />
                    <Skeleton variant="text" height={40} />
                    <Skeleton variant="text" height={40} />
                    <Skeleton variant="text" height={40} />
                    <Skeleton variant="text" height={40} />
                    <Skeleton variant="text" height={40} />
                    <Skeleton variant="text" height={40} />
                </Fragment>
            )
        } else if (this.props.error) {
            return <LoadingError />
        } else {
            return (
                <Fragment>
                    <div className="table-wrapper">
                        <Table stickyHeader aria-label="sticky table">
                            <TableHead>
                                <TableRow>
                                    {(this.props.full ? columnsFull : columnsCompact).map(column => (
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
                                            {(this.props.full ? columnsFull : columnsCompact).map(column => {
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
                    {this.state.rows.length > this.state.rowsPerPage ? (
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
                    ) : null}
                </Fragment>
            )
        }
    }
}

export default BillingTable
